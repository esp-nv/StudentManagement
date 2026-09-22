using System.ComponentModel.DataAnnotations;
using StudentManagement.Common.Constants;

namespace StudentManagement.Models;

public class Course
{
    public int Id { get; set; }

    [Required]
    [StringLength(
        CourseConstants.MaxCodeLength,
        MinimumLength = CourseConstants.MinCodeLength)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [StringLength(
        CourseConstants.MaxNameLength,
        MinimumLength = CourseConstants.MinNameLength)]
    public string Name { get; set; } = string.Empty;

    [StringLength(CourseConstants.MaxDescriptionLength)]
    public string Description { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }
}
