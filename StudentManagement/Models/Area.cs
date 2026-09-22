namespace StudentManagement.Models;

using System.ComponentModel.DataAnnotations;
using StudentManagement.Common.Constants;

public class Area
{
    public int Id { get; set; }

    [Required]
    [StringLength(
        AreaConstants.MaxCodeLength,
        MinimumLength = AreaConstants.MinCodeLength)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [StringLength(
        AreaConstants.MaxNameLength,
        MinimumLength = AreaConstants.MinNameLength)]
    public string Name { get; set; } = string.Empty;

    [StringLength(AreaConstants.MaxDescriptionLength)]
    public string Description { get; set; } = string.Empty;

    public ICollection<StudyProgram> StudyPrograms { get; set; }
        = new List<StudyProgram>();
}
