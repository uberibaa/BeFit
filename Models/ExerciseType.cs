using System.ComponentModel.DataAnnotations;

namespace BeFit.Models
{
    public class ExerciseType
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Podaj nazwę ćwiczenia.")]
        [StringLength(100, MinimumLength = 3,
            ErrorMessage = "Nazwa musi mieć od 3 do 100 znaków.")]
        [Display(Name = "Nazwa ćwiczenia")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500,
            ErrorMessage = "Opis może mieć maksymalnie 500 znaków.")]
        [Display(Name = "Opis ćwiczenia")]
        public string? Description { get; set; }
    }
}

