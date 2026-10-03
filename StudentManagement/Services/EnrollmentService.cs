using Microsoft.EntityFrameworkCore;
using StudentManagement.Common.Enums;
using StudentManagement.Data;
using StudentManagement.Models;
using StudentManagement.Services.Interfaces;

namespace StudentManagement.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly ApplicationDbContext _context;

    public EnrollmentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Enrollment?> GetByIdAsync(int id)
    {
        return await _context.Enrollments
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
    }

    public async Task<Enrollment?> CreatePendingAsync(
     int studentId,
     int courseOfferingId)
    {
        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.Id == studentId && !s.IsDeleted);

        if (student == null)
        {
            return null;
        }

        var courseOffering = await _context.CourseOfferings
            .FirstOrDefaultAsync(co =>
                co.Id == courseOfferingId &&
                !co.IsDeleted);

        if (courseOffering == null)
        {
            return null;
        }

        var existingEnrollment = await _context.Enrollments
            .FirstOrDefaultAsync(e =>
                e.StudentId == studentId &&
                e.CourseOfferingId == courseOfferingId &&
                !e.IsDeleted);

        if (existingEnrollment != null)
        {
            return null;
        }

        var enrollment = new Enrollment
        {
            StudentId = studentId,
            CourseOfferingId = courseOfferingId,
            EnrollmentDate = DateTime.UtcNow,
            Status = EnrollmentStatus.Pending,
            IsDeleted = false
        };

        _context.Enrollments.Add(enrollment);

        await _context.SaveChangesAsync();


        return enrollment;

    }


    public async Task<bool> ApproveAsync(int enrollmentId)
    {
        var enrollment = await _context.Enrollments
            .FirstOrDefaultAsync(e =>
                e.Id == enrollmentId &&
                !e.IsDeleted);

        if (enrollment == null)
        {
            return false;
        }

        if (enrollment.Status != EnrollmentStatus.Pending)
        {
            return false;
        }

        var courseOffering = await _context.CourseOfferings
            .FirstOrDefaultAsync(co =>
                co.Id == enrollment.CourseOfferingId &&
                !co.IsDeleted);

        if (courseOffering == null)
        {
            return false;
        }

        var activeEnrollmentsCount = await _context.Enrollments
            .CountAsync(e =>
                e.CourseOfferingId == courseOffering.Id &&
                e.Status == EnrollmentStatus.Active &&
                !e.IsDeleted);

        if (activeEnrollmentsCount >= courseOffering.Capacity)
        {
            return false;
        }

        enrollment.Status = EnrollmentStatus.Active;

        await _context.SaveChangesAsync();

        return true;
    }

}
