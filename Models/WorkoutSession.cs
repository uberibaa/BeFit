using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BeFit.Models
{
    public class WorkoutSession : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Podaj datę rozpoczęcia treningu.")]
        [Display(Name = "Rozpoczęcie treningu")]
        [DataType(DataType.DateTime)]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "Podaj datę zakończenia treningu.")]
        [Display(Name = "Zakończenie treningu")]
        [DataType(DataType.DateTime)]
        public DateTime EndTime { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        public IdentityUser? User { get; set; }

        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            if (StartTime == default)
            {
                yield return new ValidationResult(
                    "Podaj poprawną datę rozpoczęcia treningu.",
                    new[] { nameof(StartTime) });
            }

            if (EndTime <= StartTime)
            {
                yield return new ValidationResult(
                    "Zakończenie treningu musi być późniejsze niż rozpoczęcie.",
                    new[] { nameof(EndTime) });
            }
        }
    }
}
