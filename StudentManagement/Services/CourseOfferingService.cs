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

    // DB: CourseOffering Details изисква свързания Course.
    public async Task<CourseOffering?> GetByIdAsync(int id)
    {
        return await _context.CourseOfferings
            .AsNoTracking()
            .Include(co => co.Course)
            .FirstOrDefaultAsync(co =>
                co.Id == id &&
                !co.IsDeleted);
    }
}
