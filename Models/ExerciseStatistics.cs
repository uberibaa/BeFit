
using System.ComponentModel.DataAnnotations;

namespace BeFit.Models
{
    public class ExerciseStatistics
    {
        [Display(Name = "Nazwa ćwiczenia")]
        public string ExerciseName { get; set; } = string.Empty;

        [Display(Name = "Liczba wykonań")]
        public int ExecutionCount { get; set; }

        [Display(Name = "Łączna liczba powtórzeń")]
        public long TotalRepetitions { get; set; }

        [Display(Name = "Średnie obciążenie (kg)")]
        public decimal? AverageWeight { get; set; }

        [Display(Name = "Maksymalne obciążenie (kg)")]
        public decimal? MaxWeight { get; set; }
    }
}
