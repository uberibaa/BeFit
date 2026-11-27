
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BeFit.Models
{
    public class WorkoutSessionForm : IValidatableObject
    {
        [BindNever]
        public int Id { get; set; }

        [Required(ErrorMessage = "Podaj datę rozpoczęcia treningu.")]
        [Display(Name = "Rozpoczęcie treningu")]
        [DataType(DataType.DateTime)]
        public DateTime? StartTime { get; set; }

        [Required(ErrorMessage = "Podaj datę zakończenia treningu.")]
        [Display(Name = "Zakończenie treningu")]
        [DataType(DataType.DateTime)]
        public DateTime? EndTime { get; set; }

        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            if (StartTime == DateTime.MinValue)
            {
                yield return new ValidationResult(
                    "Podaj poprawną datę rozpoczęcia.",
                    new[] { nameof(StartTime) });
            }

            if (StartTime.HasValue && EndTime.HasValue &&
                EndTime.Value <= StartTime.Value)
            {
                yield return new ValidationResult(
                    "Zakończenie treningu musi być późniejsze niż rozpoczęcie.",
                    new[] { nameof(EndTime) });
            }
        }
    }
}
