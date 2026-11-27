
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BeFit.Models
{
    public class WorkoutExerciseForm
    {
        [BindNever]
        public int Id { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Wybierz typ ćwiczenia.")]
        [Display(Name = "Typ ćwiczenia")]
        public int ExerciseTypeId { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Wybierz sesję treningową.")]
        [Display(Name = "Sesja treningowa")]
        public int WorkoutSessionId { get; set; }

        [Required(ErrorMessage = "Podaj obciążenie.")]
        [Range(0, 1000,
            ErrorMessage = "Obciążenie musi wynosić od 0 do 1000 kg.")]
        [Display(Name = "Obciążenie (kg)")]
        public decimal? Weight { get; set; }

        [Required(ErrorMessage = "Podaj liczbę serii.")]
        [Range(1, 100,
            ErrorMessage = "Liczba serii musi wynosić od 1 do 100.")]
        [Display(Name = "Liczba serii")]
        public int? Sets { get; set; }

        [Required(ErrorMessage = "Podaj liczbę powtórzeń.")]
        [Range(1, 1000,
            ErrorMessage = "Liczba powtórzeń musi wynosić od 1 do 1000.")]
        [Display(Name = "Powtórzenia w serii")]
        public int? Repetitions { get; set; }
    }
}
