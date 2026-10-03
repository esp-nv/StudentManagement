using StudentManagement.Models;

namespace StudentManagement.Services.Interfaces;

public interface ICourseOfferingService
{
    Task<CourseOffering?> GetByIdAsync(int id);
}
