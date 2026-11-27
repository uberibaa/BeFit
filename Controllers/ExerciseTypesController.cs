using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BeFit.Models;
using BeFit.Data;
using Microsoft.AspNetCore.Authorization;

namespace BeFit.Controllers;

[Authorize(Roles = "Administrator")]
public class ExerciseTypesController : Controller
{
    private readonly ApplicationDbContext _context;

    public ExerciseTypesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: EXERCISETYPES
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View(await _context.ExerciseTypes.OrderBy(e => e.Name).ToListAsync());
    }

    // GET: EXERCISETYPES/Details/5
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var exercisetype = await _context.ExerciseTypes
            .FirstOrDefaultAsync(m => m.Id == id);
        if (exercisetype == null)
        {
            return NotFound();
        }

        return View(exercisetype);
    }

    // GET: EXERCISETYPES/Create
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // POST: EXERCISETYPES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Description")] ExerciseType exercisetype)
    {
        if (ModelState.IsValid)
        {
            _context.Add(exercisetype);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(exercisetype);
    }

    // GET: EXERCISETYPES/Edit/5
    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var exercisetype = await _context.ExerciseTypes.FindAsync(id);
        if (exercisetype == null)
        {
            return NotFound();
        }
        return View(exercisetype);
    }

    // POST: EXERCISETYPES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([FromRoute] int? id, [Bind("Id,Name,Description")] ExerciseType exercisetype)
    {
        if (id != exercisetype.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(exercisetype);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExerciseTypeExists(exercisetype.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(exercisetype);
    }

    // GET: EXERCISETYPES/Delete/5
    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var exercisetype = await _context.ExerciseTypes
            .FirstOrDefaultAsync(m => m.Id == id);
        if (exercisetype == null)
        {
            return NotFound();
        }

        return View(exercisetype);
    }

    // POST: EXERCISETYPES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed([FromRoute] int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var exercisetype = await _context.ExerciseTypes.FindAsync(id);

        if (exercisetype == null)
        {
            return NotFound();
        }

        bool isUsed = await _context.WorkoutExercises
            .AnyAsync(e => e.ExerciseTypeId == id);

        if (isUsed)
        {
            ModelState.AddModelError(string.Empty,
                "Nie można usunąć typu ćwiczenia, ponieważ jest wykorzystywany w treningach.");

            return View("Delete", exercisetype);
        }

        try
        {
            _context.ExerciseTypes.Remove(exercisetype);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError(string.Empty,
                "Nie udało się usunąć typu ćwiczenia.");

            return View("Delete", exercisetype);
        }

        return RedirectToAction(nameof(Index));
    }


    private bool ExerciseTypeExists(int? id)
    {
        return _context.ExerciseTypes.Any(e => e.Id == id);
    }
}
