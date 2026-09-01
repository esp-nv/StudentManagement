namespace StudentManagement.ViewModels;

using StudentManagement.Common.Constants;
using System.ComponentModel.DataAnnotations;

public class CreateStudentViewModel
{
    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Range(StudentConstants.MinAge, StudentConstants.MaxAge)]
    public int Age { get; set; }



    public string Course { get; set; } = string.Empty;

}
