using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models;
using StudentManagement.Services.Interfaces;

namespace StudentManagement.Services;

public class StudentService : IStudentService
{
    private readonly ApplicationDbContext _context;

    public StudentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Student>> GetAllAsync()
    {
        return await _context.Students
            .AsNoTracking()
            .Where(s => !s.IsDeleted)
            .ToListAsync();
    }

    public async Task<Student?> GetByIdAsync(int id)
    {
        return await _context.Students
            .AsNoTracking()
            .FirstOrDefaultAsync(s =>
                s.Id == id &&
                !s.IsDeleted);
    }

    public async Task CreateAsync(Student student)
    {
        student.IsDeleted = false;

        _context.Students.Add(student);

        await _context.SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(Student student)
    {
        var existingStudent = await _context.Students
            .FirstOrDefaultAsync(s =>
                s.Id == student.Id &&
                !s.IsDeleted);

        if (existingStudent == null)
        {
            return false;
        }

        existingStudent.FirstName = student.FirstName;
        existingStudent.LastName = student.LastName;
        existingStudent.Email = student.Email;
        existingStudent.DateOfBirth = student.DateOfBirth;
        existingStudent.IsDateOfBirthEstimated =
            student.IsDateOfBirthEstimated;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var student = await _context.Students
            .FirstOrDefaultAsync(s =>
                s.Id == id &&
                !s.IsDeleted);

        if (student == null)
        {
            return false;
        }

        student.IsDeleted = true;

        await _context.SaveChangesAsync();

        return true;
    }
}
