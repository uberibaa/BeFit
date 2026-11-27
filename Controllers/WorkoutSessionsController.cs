using BeFit.Data;
using BeFit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeFit.Controllers
{
    [Authorize]
    public class WorkoutSessionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public WorkoutSessionsController(
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

        private IQueryable<WorkoutSession> OwnedSessions()
        {
            var userId = CurrentUserId;

            return _context.WorkoutSessions
                .Where(s => s.UserId == userId);
        }

        // GET: WorkoutSessions
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var sessions = await OwnedSessions()
                .OrderByDescending(s => s.StartTime)
                .ToListAsync();

            return View(sessions);
        }

        // GET: WorkoutSessions/Details/5

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var session = await OwnedSessions()
                .FirstOrDefaultAsync(s => s.Id == id);

            if (session == null)
            {
                return NotFound();
            }

            var userId = CurrentUserId;
            var exercises = await _context.WorkoutExercises
                .Include(e => e.ExerciseType)
                .Where(e => e.WorkoutSessionId == session.Id &&
                            e.WorkoutSession!.UserId == userId)
                .OrderBy(e => e.Id)
                .ToListAsync();

            var viewModel = new WorkoutSessionDetailsViewModel
            {
                Session = session,
                Exercises = exercises
            };

            return View(viewModel);
        }


        // GET: WorkoutSessions/Create
        [HttpGet]
        public IActionResult Create()
        {
            var form = new WorkoutSessionForm
            {
                StartTime = DateTime.Now.AddHours(-1),
                EndTime = DateTime.Now
            };

            return View(form);
        }

        // POST: WorkoutSessions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            WorkoutSessionForm form)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }

            var session = new WorkoutSession
            {
                StartTime = form.StartTime!.Value,
                EndTime = form.EndTime!.Value,
                UserId = CurrentUserId
            };

            _context.WorkoutSessions.Add(session);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: WorkoutSessions/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var session = await OwnedSessions()
                .FirstOrDefaultAsync(s => s.Id == id);

            if (session == null)
            {
                return NotFound();
            }

            var form = new WorkoutSessionForm
            {
                Id = session.Id,
                StartTime = session.StartTime,
                EndTime = session.EndTime
            };

            return View(form);
        }

        // POST: WorkoutSessions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            [FromRoute] int id,
            WorkoutSessionForm form)
        {
            var session = await OwnedSessions()
                .FirstOrDefaultAsync(s => s.Id == id);

            if (session == null)
            {
                return NotFound();
            }

            form.Id = session.Id;

            if (!ModelState.IsValid)
            {
                return View(form);
            }

            session.StartTime = form.StartTime!.Value;
            session.EndTime = form.EndTime!.Value;

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

        // GET: WorkoutSessions/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var session = await OwnedSessions()
                .FirstOrDefaultAsync(s => s.Id == id);

            if (session == null)
            {
                return NotFound();
            }

            return View(session);
        }

        // POST: WorkoutSessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([FromRoute] int id)
        {
            var session = await OwnedSessions()
                .FirstOrDefaultAsync(s => s.Id == id);

            if (session == null)
            {
                return NotFound();
            }

            _context.WorkoutSessions.Remove(session);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
