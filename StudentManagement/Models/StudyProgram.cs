using System.ComponentModel.DataAnnotations;
using StudentManagement.Common.Constants;

namespace StudentManagement.Models;

public class StudyProgram
{
    public int Id { get; set; }

    [Required]
    public int AreaId { get; set; }

    [Required]
    [StringLength(
        StudyProgramConstants.MaxCodeLength,
        MinimumLength = StudyProgramConstants.MinCodeLength)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [StringLength(
        StudyProgramConstants.MaxNameLength,
        MinimumLength = StudyProgramConstants.MinNameLength)]
    public string Name { get; set; } = string.Empty;

    [StringLength(StudyProgramConstants.MaxDescriptionLength)]
    public string Description { get; set; } = string.Empty;

    public Area Area { get; set; } = null!;

    public ICollection<Module> Modules { get; set; } = new List<Module>();

    public bool IsDeleted { get; set; }


}
