using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentManagement.Models;
using StudentManagement.Services.Interfaces;

namespace StudentManagement.Controllers;

public class CourseOfferingsController : Controller
{
    private readonly ICourseOfferingService _courseOfferingService;
    private readonly ICourseService _courseService;

    public CourseOfferingsController(
        ICourseOfferingService courseOfferingService,
        ICourseService courseService)
    {
        _courseOfferingService = courseOfferingService;
        _courseService = courseService;
    }

    public async Task<IActionResult> Index()
    {
        var courseOfferings = await _courseOfferingService
            .GetAllAsync();

        return View(courseOfferings);
    }

    public async Task<IActionResult> Details(int? id)
    {
        var courseOffering = await GetCourseOfferingAsync(id);

        if (courseOffering == null)
        {
            return NotFound();
        }

        return View(courseOffering);
    }

    public async Task<IActionResult> Create()
    {
        await LoadCoursesAsync();

        return View(new CourseOffering());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CourseOffering courseOffering)
    {
        if (!ModelState.IsValid)
        {
            return await ReturnToFormAsync(courseOffering);
        }

        var created = await _courseOfferingService
            .CreateAsync(courseOffering);

        if (created == null)
        {
            ModelState.AddModelError(
                nameof(CourseOffering.CourseId),
                "The selected course is not available.");

            return await ReturnToFormAsync(courseOffering);
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        var courseOffering = await GetCourseOfferingAsync(id);

        if (courseOffering == null)
        {
            return NotFound();
        }

        await LoadCoursesAsync(courseOffering.CourseId);

        return View(courseOffering);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        CourseOffering courseOffering)
    {
        if (id != courseOffering.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return await ReturnToFormAsync(courseOffering);
        }

        var updated = await _courseOfferingService
            .UpdateAsync(courseOffering);

        if (!updated)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        var courseOffering = await GetCourseOfferingAsync(id);

        if (courseOffering == null)
        {
            return NotFound();
        }

        return View(courseOffering);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var deleted = await _courseOfferingService
            .DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task<CourseOffering?> GetCourseOfferingAsync(
        int? id)
    {
        if (id == null)
        {
            return null;
        }

        return await _courseOfferingService
            .GetByIdAsync(id.Value);
    }

    private async Task LoadCoursesAsync(
        int? selectedCourseId = null)
    {
        var courses = await _courseService.GetAllAsync();

        ViewBag.Courses = new SelectList(
            courses,
            "Id",
            "Name",
            selectedCourseId);
    }

    private async Task<IActionResult> ReturnToFormAsync(
        CourseOffering courseOffering)
    {
        await LoadCoursesAsync(courseOffering.CourseId);

        return View(courseOffering);
    }
}
