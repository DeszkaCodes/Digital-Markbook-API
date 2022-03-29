using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Services;
using SchoolAPI.Classes;
using SchoolAPI.Models;

namespace SchoolAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentController : ControllerBase
{
    private StudentService _service;

    private SchoolService _schoolService;

    public StudentController(StudentService service, SchoolService schoolService)
    {
        _service = service;
        _schoolService = schoolService;
    }

    [HttpGet]
    public ActionResult<List<Student>> GetMany(int? limit)
    {
        if(limit is not null && limit > 0)
            return _service.GetAll().Take((int)limit).ToList();
        
        return _service.GetAll().ToList();
    }

    [HttpGet("{id}")]
    public ActionResult<Student> Get(Guid id){
        var student = _service.GetById(id);

        if(student is null)
            return NotFound();

        return student;
    }

    [HttpPost]
    public ActionResult<Student> Create(string name, string dateofbirth, Gender gender, Guid schoolId)
    {
        var school = _schoolService.GetById(schoolId);

        if(school is null)
            return NotFound();

        var date = DateTime.Now;

        if(!DateTime.TryParse(dateofbirth, out date))
            return BadRequest();

        Guid id;

        do
        {
            id = Guid.NewGuid();
        }while(_service.IdExists(id));

        var student = new Student()
            {
                Id = id,
                Name = name,
                Gender = gender,
                DateOfBirth = date,
                School = school
            };

        _service.Create(student);

        return student;
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var student = _service.GetById(id);

        if(student is not null)
        {
            _service.DeleteById(id);
            return Ok();
        }

        return NotFound();        
    }

    [HttpPut("{id}/name")]
    public IActionResult UpdateName(Guid id, string name)
    {
        if(_service.GetById(id) is null)
            return NotFound();

        _service.UpdateName(id, name);

        return Ok();
    }

    [HttpPut("{id}/gender")]
    public IActionResult UpdateGender(Guid id, Gender gender)
    {
        _service.UpdateGender(id, gender);

        return Ok();
    }

    [HttpPut("{id}/dateofbirth")]
    public IActionResult UpdateDateOfBirth(Guid id, string dateofbirth)
    {
        var date = DateTime.Now;

        if(!DateTime.TryParse(dateofbirth, out date))
            return BadRequest();

        if(_service.GetById(id) is null)
            return NotFound();

        _service.UpdateBirthDate(id, date);

        return Ok();
    }

    [HttpPut("{id}/school")]
    public IActionResult UpdateSchool(Guid id, Guid schoolId)
    {
        try
        {
            if(_service.GetById(id) is null)
                return BadRequest();

            _service.UpdateSchool(id, schoolId);

            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }
}