namespace BeFit.Models
{
    public class WorkoutSessionDetailsViewModel
    {
        public required WorkoutSession Session { get; set; }

        public List<WorkoutExercise> Exercises { get; set; } = new();
    }
}
