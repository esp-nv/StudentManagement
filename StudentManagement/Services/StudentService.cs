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
            .ToListAsync();
    }

    public async Task<Student?> GetByIdAsync(int id)
    {
        return await _context.Students.FindAsync(id);
    }

    public async Task CreateAsync(Student student)
    {
        _context.Students.Add(student);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(Student student)
    {
        var existingStudent = await _context.Students
            .FindAsync(student.Id);

        if (existingStudent == null)
        {
            return false;
        }

        existingStudent.FirstName = student.FirstName;
        existingStudent.LastName = student.LastName;
        existingStudent.Email = student.Email;
        existingStudent.Course = student.Course;
        existingStudent.Age = student.Age;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var student = await _context.Students.FindAsync(id);

        if (student == null)
        {
            return false;
        }

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();

        return true;
    }
}
