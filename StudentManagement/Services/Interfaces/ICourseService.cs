using StudentManagement.Models;

namespace StudentManagement.Services.Interfaces;

public interface ICourseService
{
    Task<IEnumerable<Course>> GetAllAsync();

    Task<IEnumerable<Course>> GetAvailableForOfferingAsync();

    Task<Course?> GetByIdAsync(int id);

    Task<Course?> CreateAsync(Course course);

    Task<bool> UpdateAsync(Course course);

    Task<bool> DeleteAsync(int id);
}
