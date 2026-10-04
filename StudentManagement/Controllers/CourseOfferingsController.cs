using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Services.Interfaces;
using StudentManagement.ViewModels;

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
        var viewModel = new CourseOfferingViewModel();

        await PrepareViewModelAsync(viewModel);

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CourseOfferingViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            await PrepareViewModelAsync(viewModel);

            return View(viewModel);
        }

        var courseOffering = new CourseOffering
        {
            CourseId = viewModel.CourseId,
            StartDate = viewModel.StartDate.Value,
            EndDate = viewModel.EndDate.Value,
            EnrollmentStartDate =
                viewModel.EnrollmentStartDate.Value,
            EnrollmentEndDate =
                viewModel.EnrollmentEndDate.Value,
            Capacity = viewModel.Capacity
        };

        var created = await _courseOfferingService
            .CreateAsync(courseOffering);

        if (created == null)
        {
            ModelState.AddModelError(
                nameof(viewModel.CourseId),
                "The selected course is not available.");

            await PrepareViewModelAsync(viewModel);

            return View(viewModel);
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

        var viewModel = new CourseOfferingViewModel
        {
            CourseId = courseOffering.CourseId,
            StartDate = courseOffering.StartDate,
            EndDate = courseOffering.EndDate,
            EnrollmentStartDate =
                courseOffering.EnrollmentStartDate,
            EnrollmentEndDate =
                courseOffering.EnrollmentEndDate,
            Capacity = courseOffering.Capacity
        };

        await PrepareViewModelAsync(viewModel);

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        CourseOfferingViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            await PrepareViewModelAsync(viewModel);

            return View(viewModel);
        }

        var courseOffering = new CourseOffering
        {
            Id = id,
            CourseId = viewModel.CourseId,
            StartDate = viewModel.StartDate.Value,
            EndDate = viewModel.EndDate.Value,
            EnrollmentStartDate =
                viewModel.EnrollmentStartDate.Value,
            EnrollmentEndDate =
                viewModel.EnrollmentEndDate.Value,
            Capacity = viewModel.Capacity
        };

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

    private async Task<CourseOffering?> GetCourseOfferingAsync(int? id)
    {
        if (id == null)
        {
            return null;
        }

        return await _courseOfferingService
            .GetByIdAsync(id.Value);
    }

    private async Task PrepareViewModelAsync(
        CourseOfferingViewModel viewModel)
    {
        var courses = await _courseService
            .GetAvailableForOfferingAsync();

        viewModel.Courses = courses
            .Select(c => new CourseSelectItemViewModel
            {
                Id = c.Id,
                Code = c.Code,
                Name = c.Name
            })
            .ToList();
    }
}
