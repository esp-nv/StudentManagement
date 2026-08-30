using StudentManagement.Models;

namespace StudentManagement.Services.Interfaces;

public interface IStudentService
{
    Task<List<Student>> GetAllAsync();

    Task<Student?> GetByIdAsync(int id);

    Task CreateAsync(Student student);

    Task<bool> UpdateAsync(Student student);

    Task<bool> DeleteAsync(int id);
}
