using System.ComponentModel.DataAnnotations;
using StudentManagement.Common.Enums;

namespace StudentManagement.Models;

public class Enrollment
{
    public int Id { get; set; }

    [Required]
    public int StudentId { get; set; }

    [Required]
    public int CourseOfferingId { get; set; }

    [Required]
    public DateTime EnrollmentDate { get; set; }

    [Required]
    public EnrollmentStatus Status { get; set; }

    public bool IsDeleted { get; set; }

    public Student Student { get; set; } = null!;

    public CourseOffering CourseOffering { get; set; } = null!;
}
