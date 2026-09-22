using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Models;

public class Course
{
    public int Id { get; set; }

    [Required]
    public string Code { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }
}
