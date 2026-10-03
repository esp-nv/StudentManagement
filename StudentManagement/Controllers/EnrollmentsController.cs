using Microsoft.AspNetCore.Mvc;
using StudentManagement.Services.Interfaces;

namespace StudentManagement.Controllers;

public class EnrollmentsController : Controller
{
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentsController(IEnrollmentService enrollmentService)
    {
        _enrollmentService = enrollmentService;
    }


}
