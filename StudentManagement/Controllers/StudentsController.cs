using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models;
using StudentManagement.Services.Interfaces;
using StudentManagement.ViewModels;


namespace StudentManagement.Controllers;

public class StudentsController : Controller
{
   private readonly IStudentService _studentService;


    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }


    public async Task<IActionResult> Index()
    {
        var students = await _studentService.GetAllAsync();

        return View(students);
    }


    public IActionResult Create()
    {
        return View(new CreateStudentViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateStudentViewModel student)

    {
        if (!ModelState.IsValid)
        {
            return View(student);
        }

        var newStudent = new Student
        {
            FirstName = student.FirstName,
            LastName = student.LastName,
            Email = student.Email,
            Course = student.Course,
            Age = student.Age
        };


        await _studentService.CreateAsync(newStudent);

        return RedirectToAction(nameof(Index));
    }


    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var student = await _studentService.GetByIdAsync(id.Value);


        if (student == null)
        {
            return NotFound();
        }

        return View(student);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Student student)
    {
        if (id != student.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(student);
        }

        var updated = await _studentService.UpdateAsync(student);

        if (!updated)
        {
            return NotFound();
        }


        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var student = await _studentService.GetByIdAsync(id.Value);


        if (student == null)
        {
            return NotFound();
        }

        return View(student);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var deleted = await _studentService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }



}
