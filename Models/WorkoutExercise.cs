using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BeFit.Models
{
    public class WorkoutExercise
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Wybierz typ ćwiczenia.")]
        [Display(Name = "Typ ćwiczenia")]
        public int ExerciseTypeId { get; set; }

        public ExerciseType? ExerciseType { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Wybierz sesję treningową.")]
        [Display(Name = "Sesja treningowa")]
        public int WorkoutSessionId { get; set; }

        public WorkoutSession? WorkoutSession { get; set; }

        [Range(0, 1000,
            ErrorMessage = "Obciążenie musi wynosić od 0 do 1000 kg.")]
        [Precision(6, 2)]
        [Display(Name = "Obciążenie (kg)")]
        public decimal Weight { get; set; }

        [Range(1, 100,
            ErrorMessage = "Liczba serii musi wynosić od 1 do 100.")]
        [Display(Name = "Liczba serii")]
        public int Sets { get; set; }

        [Range(1, 1000,
            ErrorMessage = "Liczba powtórzeń musi wynosić od 1 do 1000.")]
        [Display(Name = "Powtórzenia w serii")]
        public int Repetitions { get; set; }
    }
}

