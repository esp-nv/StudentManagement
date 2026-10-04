using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models;
using StudentManagement.Services.Interfaces;

namespace StudentManagement.Services;

public class CourseOfferingService : ICourseOfferingService
{
    private readonly ApplicationDbContext _context;

    public CourseOfferingService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CourseOffering>> GetAllAsync()
    {
        return await _context.CourseOfferings
            .AsNoTracking()
            .Include(co => co.Course)
            .Where(co =>
                !co.IsDeleted &&
                !co.Course.IsDeleted)
            .OrderBy(co => co.StartDate)
            .ToListAsync();
    }

    public async Task<CourseOffering?> GetByIdAsync(int id)
    {
        return await _context.CourseOfferings
            .AsNoTracking()
            .Include(co => co.Course)
            .FirstOrDefaultAsync(co =>
                co.Id == id &&
                !co.IsDeleted &&
                !co.Course.IsDeleted);
    }

    public async Task<CourseOffering?> CreateAsync(
        CourseOffering courseOffering)
    {
        if (!IsValidDates(courseOffering))
        {
            return null;
        }

        var courseExists = await _context.Courses
            .AnyAsync(c =>
                c.Id == courseOffering.CourseId &&
                !c.IsDeleted);

        if (!courseExists)
        {
            return null;
        }

        courseOffering.IsDeleted = false;

        _context.CourseOfferings.Add(courseOffering);

        await _context.SaveChangesAsync();

        return courseOffering;
    }

    public async Task<bool> UpdateAsync(
        CourseOffering courseOffering)
    {
        if (!IsValidDates(courseOffering))
        {
            return false;
        }

        var existingCourseOffering = await _context.CourseOfferings
            .FirstOrDefaultAsync(co =>
                co.Id == courseOffering.Id &&
                !co.IsDeleted);

        if (existingCourseOffering == null)
        {
            return false;
        }

        var courseExists = await _context.Courses
            .AnyAsync(c =>
                c.Id == courseOffering.CourseId &&
                !c.IsDeleted);

        if (!courseExists)
        {
            return false;
        }

        existingCourseOffering.CourseId =
            courseOffering.CourseId;

        existingCourseOffering.StartDate =
            courseOffering.StartDate;

        existingCourseOffering.EndDate =
            courseOffering.EndDate;

        existingCourseOffering.EnrollmentStartDate =
            courseOffering.EnrollmentStartDate;

        existingCourseOffering.EnrollmentEndDate =
            courseOffering.EnrollmentEndDate;

        existingCourseOffering.Capacity =
            courseOffering.Capacity;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var courseOffering = await _context.CourseOfferings
            .FirstOrDefaultAsync(co =>
                co.Id == id &&
                !co.IsDeleted);

        if (courseOffering == null)
        {
            return false;
        }

        courseOffering.IsDeleted = true;

        await _context.SaveChangesAsync();

        return true;
    }

    private static bool IsValidDates(
        CourseOffering courseOffering)
    {
        return courseOffering.StartDate <= courseOffering.EndDate
            && courseOffering.EnrollmentStartDate
                <= courseOffering.EnrollmentEndDate
            && courseOffering.EnrollmentStartDate
                <= courseOffering.StartDate;
    }
}
