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
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string Course { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }
    }
}
