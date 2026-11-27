using BeFit.Data;
using BeFit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BeFit.Controllers
{
    [Authorize]
    public class WorkoutExercisesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public WorkoutExercisesController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private string CurrentUserId =>
            _userManager.GetUserId(User)
            ?? throw new InvalidOperationException(
                "Nie znaleziono zalogowanego użytkownika.");

        private IQueryable<WorkoutExercise> OwnedExercises()
        {
            var userId = CurrentUserId;

            return _context.WorkoutExercises
                .Include(e => e.ExerciseType)
                .Include(e => e.WorkoutSession)
                .Where(e => e.WorkoutSession!.UserId == userId);
        }

        // GET: WorkoutExercises
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var exercises = await OwnedExercises()
                .OrderByDescending(e => e.WorkoutSession!.StartTime)
                .ThenBy(e => e.ExerciseType!.Name)
                .ToListAsync();

            return View(exercises);
        }

        // GET: WorkoutExercises/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var exercise = await OwnedExercises()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (exercise == null)
            {
                return NotFound();
            }

            return View(exercise);
        }

        // GET: WorkoutExercises/Create
        [HttpGet]
        public async Task<IActionResult> Create(int? sessionId)
        {
            var form = new WorkoutExerciseForm
            {
                WorkoutSessionId = sessionId ?? 0,
                Sets = 3,
                Repetitions = 10
            };

            if (sessionId.HasValue)
            {
                var userId = CurrentUserId;

                bool isOwned = await _context.WorkoutSessions
                    .AnyAsync(s => s.Id == sessionId.Value
                        && s.UserId == userId);

                if (!isOwned)
                {
                    return NotFound();
                }
            }

            await PopulateSelectLists(form);

            return View(form);
        }

        // POST: WorkoutExercises/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            WorkoutExerciseForm form)
        {
            await ValidateSelections(form);

            if (!ModelState.IsValid)
            {
                await PopulateSelectLists(form);
                return View(form);
            }

            var exercise = new WorkoutExercise
            {
                ExerciseTypeId = form.ExerciseTypeId,
                WorkoutSessionId = form.WorkoutSessionId,
                Weight = form.Weight!.Value,
                Sets = form.Sets!.Value,
                Repetitions = form.Repetitions!.Value
            };

            _context.WorkoutExercises.Add(exercise);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: WorkoutExercises/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var exercise = await OwnedExercises()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (exercise == null)
            {
                return NotFound();
            }

            var form = new WorkoutExerciseForm
            {
                Id = exercise.Id,
                ExerciseTypeId = exercise.ExerciseTypeId,
                WorkoutSessionId = exercise.WorkoutSessionId,
                Weight = exercise.Weight,
                Sets = exercise.Sets,
                Repetitions = exercise.Repetitions
            };

            await PopulateSelectLists(form);

            return View(form);
        }

        // POST: WorkoutExercises/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            [FromRoute] int id,
            WorkoutExerciseForm form)
        {
            var exercise = await OwnedExercises()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (exercise == null)
            {
                return NotFound();
            }

            form.Id = exercise.Id;

            await ValidateSelections(form);

            if (!ModelState.IsValid)
            {
                await PopulateSelectLists(form);
                return View(form);
            }

            exercise.ExerciseTypeId = form.ExerciseTypeId;
            exercise.WorkoutSessionId = form.WorkoutSessionId;
            exercise.Weight = form.Weight!.Value;
            exercise.Sets = form.Sets!.Value;
            exercise.Repetitions = form.Repetitions!.Value;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: WorkoutExercises/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var exercise = await OwnedExercises()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (exercise == null)
            {
                return NotFound();
            }

            return View(exercise);
        }

        // POST: WorkoutExercises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(
            [FromRoute] int id)
        {
            var exercise = await OwnedExercises()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (exercise == null)
            {
                return NotFound();
            }

            _context.WorkoutExercises.Remove(exercise);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private async Task ValidateSelections(
            WorkoutExerciseForm form)
        {
            var userId = CurrentUserId;

            bool sessionExists = await _context.WorkoutSessions
                .AnyAsync(s => s.Id == form.WorkoutSessionId
                    && s.UserId == userId);

            if (!sessionExists)
            {
                ModelState.AddModelError(
                    nameof(form.WorkoutSessionId),
                    "Wybierz własną, istniejącą sesję treningową.");
            }

            bool exerciseTypeExists = await _context.ExerciseTypes
                .AnyAsync(e => e.Id == form.ExerciseTypeId);

            if (!exerciseTypeExists)
            {
                ModelState.AddModelError(
                    nameof(form.ExerciseTypeId),
                    "Wybierz istniejący typ ćwiczenia.");
            }

            if (form.Weight.HasValue &&
                decimal.Round(form.Weight.Value, 2) != form.Weight.Value)
            {
                ModelState.AddModelError(
                    nameof(form.Weight),
                    "Podaj maksymalnie dwa miejsca po przecinku.");
            }
        }

        private async Task PopulateSelectLists(
            WorkoutExerciseForm form)
        {
            var userId = CurrentUserId;

            var exerciseTypes = await _context.ExerciseTypes
                .OrderBy(e => e.Name)
                .ToListAsync();

            var sessions = await _context.WorkoutSessions
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.StartTime)
                .ToListAsync();

            ViewData["ExerciseTypeId"] = new SelectList(
                exerciseTypes,
                "Id",
                "Name",
                form.ExerciseTypeId);

            ViewData["WorkoutSessionId"] = new SelectList(
                sessions.Select(s => new
                {
                    s.Id,
                    Label =
                        $"{s.StartTime:dd.MM.yyyy HH:mm} - " +
                        $"{s.EndTime:dd.MM.yyyy HH:mm}"
                }),
                "Id",
                "Label",
                form.WorkoutSessionId);
        }
    }
}
