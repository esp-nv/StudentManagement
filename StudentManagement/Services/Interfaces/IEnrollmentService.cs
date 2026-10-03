using StudentManagement.Models;

namespace StudentManagement.Services.Interfaces;

public interface IEnrollmentService
{
    Task<Enrollment?> GetByIdAsync(int id);

    Task<Enrollment?> CreatePendingAsync(
        int studentId,
        int courseOfferingId);

    Task<bool> ApproveAsync(int enrollmentId);
}
