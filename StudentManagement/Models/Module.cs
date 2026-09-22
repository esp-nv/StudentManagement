using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Models;

public class Module
{
    public int Id { get; set; }

    [Required]
    public int StudyProgramId { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public StudyProgram StudyProgram { get; set; } = null!;

    public bool IsDeleted { get; set; }

}
