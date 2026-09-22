using System.ComponentModel.DataAnnotations;
using StudentManagement.Common.Constants;


namespace StudentManagement.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        [StringLength(StudentConstants.MaxFirstNameLength,
            MinimumLength = StudentConstants.MinFirstNameLength)]
        public string FirstName { get; set; } = string.Empty;


[Required]
[StringLength(
    StudentConstants.MaxLastNameLength,
    MinimumLength = StudentConstants.MinLastNameLength)]
public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(StudentConstants.MaxEmailLength)]
        public string Email { get; set; } = string.Empty;



       // public int Age { get; set; }


        public DateTime DateOfBirth { get; set; }

        public bool IsDateOfBirthEstimated { get; set; }

        public bool IsDeleted { get; set; }


    }
}
