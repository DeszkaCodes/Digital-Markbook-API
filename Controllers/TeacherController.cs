using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Models;
using SchoolAPI.Services;
using SchoolAPI.Generators;

namespace SchoolAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class TeacherController : ControllerBase
{
    private TeacherService _service;

    public TeacherController(TeacherService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<List<Teacher>> GetAll() =>
        _service.GetAll().ToList();

    [HttpGet("{id}")]
    public ActionResult<Teacher> Get(Guid id){
        var teacher = _service.GetById(id);

        if(teacher is null)
            return NotFound();

        return teacher;
    }
}