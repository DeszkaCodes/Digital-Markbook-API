using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Models;
using SchoolAPI.Services;
using SchoolAPI.Generators;
using SchoolAPI.Classes;

namespace SchoolAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class TeacherController : ControllerBase
{
    private TeacherService _service;
    private SchoolService _schoolService;

    public TeacherController(TeacherService service, SchoolService schoolService)
    {
        _service = service;
        _schoolService = schoolService;
    }

    [HttpGet]
    public ActionResult<List<Teacher>> GetAll(int? limit)
    {
        if (limit is not null && limit > 0)
            return _service.GetAll().Take((int)limit).ToList();

        return _service.GetAll().ToList();
    }

    [HttpGet("{id}")]
    public ActionResult<Teacher> Get(Guid id) {
        var teacher = _service.GetById(id);

        if (teacher is null)
            return NotFound();

        return teacher;
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var teacher = _service.GetById(id);
        if (teacher is null)
            return NotFound();

        _service.DeleteById(id);

        return Ok();
    }

    [HttpPost]
    public ActionResult<Teacher> Create(string name, string dateOfBirth, Gender gender, Guid schoolId)
    {
        var school = _schoolService.GetById(schoolId);

        if (school is null)
            return NotFound();

        var date = DateTime.Now;

        if (!DateTime.TryParse(dateOfBirth, out date))
            return BadRequest();

        Guid id;

        do
        {
            id = Guid.NewGuid();
        } while (_service.IdExists(id));

        var teacher = new Teacher()
        {
            Id = id,
            Name = name.ToTitleCase(),
            DateOfBirth = date,
            Gender = gender,
            School = school
        };

        _service.Create(teacher);

        return teacher;
    }

    [HttpPatch("{id}/name")]
    public IActionResult UpdateName(Guid id, string name)
    {
        var teacher = _service.GetById(id);

        if (teacher is null)
            return NotFound();

        _service.UpdateName(id, name.ToTitleCase());

        return Ok();
    }

    [HttpPatch("{id}/gender")]
    public IActionResult UpdateGender(Guid id, Gender gender)
    {
        var teacher = _service.GetById(id);

        if (teacher is null)
            return NotFound();

        _service.UpdateGender(id, gender);

        return Ok();
    }

    [HttpPatch("{id}/dateofbirth")]
    public IActionResult UpdateDateOfBirth(Guid id, string birthDate)
    {
        var teacher = _service.GetById(id);

        if (teacher is null)
            return NotFound();

        var date = DateTime.Now;

        if (!DateTime.TryParse(birthDate, out date))
            return BadRequest();

        _service.UpdateBirthDate(id, date);

        return Ok();
    }

    [HttpPatch("{id}/school")]
    public IActionResult UpdateSchool(Guid id, Guid schoolId)
    {
        var teacher = _service.GetById(id);
        var school = _schoolService.GetById(schoolId);

        if (teacher is null || school is null)
            return NotFound();

        _service.UpdateSchool(id, schoolId);

        return Ok();
    }
}