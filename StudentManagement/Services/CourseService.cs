using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models;
using StudentManagement.Services.Interfaces;

namespace StudentManagement.Services;

public class CourseService : ICourseService
{
    private readonly ApplicationDbContext _context;

    public CourseService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Course>> GetAllAsync()
    {
        return await _context.Courses
            .AsNoTracking()
            .Where(c => !c.IsDeleted)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Course>> GetAvailableForOfferingAsync()
    {
        return await _context.Courses
            .AsNoTracking()
            .Where(c => !c.IsDeleted)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<Course?> GetByIdAsync(int id)
    {
        return await _context.Courses
            .AsNoTracking()
            .FirstOrDefaultAsync(c =>
                c.Id == id &&
                !c.IsDeleted);
    }

    public async Task<Course?> CreateAsync(Course course)
    {
        course.IsDeleted = false;

        _context.Courses.Add(course);

        await _context.SaveChangesAsync();

        return course;
    }

    public async Task<bool> UpdateAsync(Course course)
    {
        var existingCourse = await _context.Courses
            .FirstOrDefaultAsync(c =>
                c.Id == course.Id &&
                !c.IsDeleted);

        if (existingCourse == null)
        {
            return false;
        }

        existingCourse.Code = course.Code;
        existingCourse.Name = course.Name;
        existingCourse.Description = course.Description;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var course = await _context.Courses
            .FirstOrDefaultAsync(c =>
                c.Id == id &&
                !c.IsDeleted);

        if (course == null)
        {
            return false;
        }

        course.IsDeleted = true;

        await _context.SaveChangesAsync();

        return true;
    }
}
