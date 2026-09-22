using System.ComponentModel.DataAnnotations;
using StudentManagement.Common.Constants;

namespace StudentManagement.Models;

public class Module
{
    public int Id { get; set; }

    [Required]
    public int StudyProgramId { get; set; }

    [Required]
    [StringLength(
        ModuleConstants.MaxNameLength,
        MinimumLength = ModuleConstants.MinNameLength)]
    public string Name { get; set; } = string.Empty;

    [StringLength(ModuleConstants.MaxDescriptionLength)]
    public string Description { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }

    public StudyProgram StudyProgram { get; set; } = null!;

    public ICollection<Course> Courses { get; set; } = new List<Course>();


}
