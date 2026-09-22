using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Models;

public class CourseOffering : IValidatableObject
{
    public int Id { get; set; }

    [Required]
    public int CourseId { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    public int Year => StartDate.Year;

    [Range(1, int.MaxValue)]
    public int Capacity { get; set; }

    public bool IsDeleted { get; set; }

    public Course Course { get; set; } = null!;

    public IEnumerable<ValidationResult> Validate(
        ValidationContext validationContext)
    {
        if (EndDate < StartDate)
        {
            yield return new ValidationResult(
                "End date cannot be before start date.",
                new[] { nameof(EndDate) });
        }
    }
}
