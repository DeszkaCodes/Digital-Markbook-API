using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Services;
using SchoolAPI.Classes;
using SchoolAPI.Models;

namespace SchoolAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class SchoolController : ControllerBase
{
    private SchoolService _service;
    private TeacherService _teacherService;

    public SchoolController(SchoolService service, TeacherService teacherService)
    {
        _service = service;
        _teacherService = teacherService;
    }

    [HttpGet]
    public ActionResult<List<School>> GetAll(int? limit) 
    {
        if(limit is null)
            return _service.GetAll().ToList();
            
        if(limit <= 0)
            return BadRequest();
        
        return _service.GetAll().Take((int)limit).ToList();
    }
    
    [HttpGet("{id}")]
    public ActionResult<School> Get(Guid id)
    {
        var school = _service.GetById(id);

        if(school is null)
            return NotFound();

        return school;
    }

    [HttpGet("{id}/teachers")]
    public ActionResult<IEnumerable<Teacher>> GetAllTeachers(Guid id)
    {
        var school = _service.GetById(id);

        if (school is null)
            return NotFound();

        return _service.GetTeachersById(id).ToList();
    }

    [HttpPost]
    public ActionResult<School> Create(string name, SchoolType type)
    {
        Guid id;

        do
        {
            id = Guid.NewGuid();
        } while(_service.IdExists(id));

        var school = new School()
            {
                Name = name.ToTitleCase(),
                Type = type
            };

        _service.Create(school);

        return school;
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var school = _service.GetById(id);

        if(school is null)
            return NotFound();

        _service.DeleteById(id);

        return Ok();
    }

    [HttpPatch("{id}/name")]
    public IActionResult UpdateName(Guid id, string name)
    {
        if(_service.GetById(id) is null)
            return NotFound();

        _service.UpdateName(id, name);

        return Ok();
    }

    [HttpGet("{id}/name")]
    public ActionResult<string> GetName(Guid id)
    {
        var school = _service.GetById(id);

        if (school is null || school.Name is null)
            return NotFound();

        return school.Name;
    }

    [HttpPatch("{id}/type")]
    public IActionResult UpdateType(Guid id, SchoolType type)
    {
        if(_service.GetById(id) is null)
            return NotFound();

        _service.UpdateType(id, type);

        return Ok();
    }

    [HttpGet("{id}/type")]
    public ActionResult<SchoolType> GetType(Guid id)
    {
        var school = _service.GetById(id);

        if (school is null)
            return NotFound();

        return school.Type;
    }
}
