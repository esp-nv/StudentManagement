using Microsoft.AspNetCore.Mvc;
using StudentManagement.Services.Interfaces;

namespace StudentManagement.Controllers;

public class CourseOfferingsController : Controller
{
    private readonly ICourseOfferingService _courseOfferingService;

    public CourseOfferingsController(
        ICourseOfferingService courseOfferingService)
    {
        _courseOfferingService = courseOfferingService;
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var courseOffering = await _courseOfferingService
            .GetByIdAsync(id.Value);

        if (courseOffering == null)
        {
            return NotFound();
        }

        return View(courseOffering);
    }

}
