using System.ComponentModel.DataAnnotations;

namespace StudentManagement.ViewModels;

public class CourseOfferingViewModel : IValidatableObject
{
    [Required]
    public int CourseId { get; set; }

    [Required]
    public DateTime? StartDate { get; set; }

    [Required]
    public DateTime? EndDate { get; set; }

    [Required]
    public DateTime? EnrollmentStartDate { get; set; }

    [Required]
    public DateTime? EnrollmentEndDate { get; set; }

    [Range(1, int.MaxValue)]
    public int Capacity { get; set; }

    public IEnumerable<CourseSelectItemViewModel> Courses { get; set; }
        = new List<CourseSelectItemViewModel>();

    public IEnumerable<ValidationResult> Validate(
        ValidationContext validationContext)
    {
        if (StartDate.HasValue &&
            EndDate.HasValue &&
            EndDate < StartDate)
        {
            yield return new ValidationResult(
                "End date cannot be before start date.",
                new[] { nameof(EndDate) });
        }

        if (EnrollmentStartDate.HasValue &&
            EnrollmentEndDate.HasValue &&
            EnrollmentEndDate < EnrollmentStartDate)
        {
            yield return new ValidationResult(
                "Enrollment end date cannot be before enrollment start date.",
                new[] { nameof(EnrollmentEndDate) });
        }

        if (EnrollmentStartDate.HasValue &&
            StartDate.HasValue &&
            EnrollmentStartDate > StartDate)
        {
            yield return new ValidationResult(
                "Enrollment start date cannot be after course start date.",
                new[] { nameof(EnrollmentStartDate) });
        }
    }
}

public class CourseSelectItemViewModel
{
    public int Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
}
