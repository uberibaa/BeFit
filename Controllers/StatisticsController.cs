using BeFit.Data;
using BeFit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeFit.Controllers
{
    [Authorize]
    public class StatisticsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public StatisticsController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            if (userId == null)
            {
                return Challenge();
            }

            var now = DateTime.Now;
            var startDate = now.AddDays(-28);

            var results = await _context.WorkoutExercises
                .Where(e =>
                    e.WorkoutSession!.UserId == userId &&
                    e.WorkoutSession.StartTime >= startDate &&
                    e.WorkoutSession.StartTime <= now)
                .GroupBy(e => e.ExerciseTypeId)
                .Select(g => new
                {
                    ExerciseTypeId = g.Key,
                    ExecutionCount = g.Count(),
                    TotalRepetitions = g.Sum(
                        e => (long)e.Sets * e.Repetitions),
                    AverageWeight = g.Average(e => e.Weight),
                    MaxWeight = g.Max(e => e.Weight)
                })
                .ToDictionaryAsync(e => e.ExerciseTypeId);

            var exerciseTypes = await _context.ExerciseTypes
                .OrderBy(e => e.Name)
                .ToListAsync();

            var statistics = exerciseTypes.Select(e =>
            {
                results.TryGetValue(e.Id, out var result);

                return new ExerciseStatistics
                {
                    ExerciseName = e.Name,
                    ExecutionCount = result?.ExecutionCount ?? 0,
                    TotalRepetitions = result?.TotalRepetitions ?? 0,
                    AverageWeight = result?.AverageWeight,
                    MaxWeight = result?.MaxWeight
                };
            }).ToList();

            ViewData["StartDate"] = startDate;
            ViewData["EndDate"] = now;

            return View(statistics);
        }
    }
}
