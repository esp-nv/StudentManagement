using StudentManagement.Models;

namespace StudentManagement.Services.Interfaces;

public interface ICourseOfferingService
{
    Task<IEnumerable<CourseOffering>> GetAllAsync();

    Task<CourseOffering?> GetByIdAsync(int id);

    Task<CourseOffering?> CreateAsync(
        CourseOffering courseOffering);

    Task<bool> UpdateAsync(
        CourseOffering courseOffering);

    Task<bool> DeleteAsync(int id);
}
