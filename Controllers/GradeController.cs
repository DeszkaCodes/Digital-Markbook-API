using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Models;
using SchoolAPI.Services;

namespace SchoolAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class GradeController : BaseController
{
    private readonly GradeService _service;

    public GradeController(GradeService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Grade>> GetAll([FromQuery] int? limit)
    {
        return null;
    }
}
