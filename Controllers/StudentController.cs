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
        if(limit is null)
            return _service.GetAll().ToList();
            
        if(limit <= 0)
            return BadRequest();
            
        return _service.GetAll().Take((int)limit).ToList();
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
                Name = name.ToTitleCase(),
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
        var student = _service.GetById(id);

        if (student is null || student.Name is null)
            return NotFound();

        return Ok(new {
            name = student.Name
        });
    }

    [HttpPatch("{id}/gender")]
    public IActionResult UpdateGender(Guid id, Gender gender)
    {
        _service.UpdateGender(id, gender);

        return Ok();
    }

    [HttpGet("{id}/gender")]
    public ActionResult<Gender> GetGender(Guid id)
    {
        var student = _service.GetById(id);

        if (student is null)
            return NotFound();

        return Ok(new {
            gender = student.Gender
        });
    }

    [HttpPatch("{id}/dateofbirth")]
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

    [HttpGet("{id}/dateofbirth")]
    public ActionResult<DateTime> GetDateOfBirth(Guid id)
    {
        var student = _service.GetById(id);

        if (student is null)
            return NotFound();

        return student.DateOfBirth;
    }

    [HttpPatch("{id}/school")]
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

    [HttpGet("{id}/school")]
    public ActionResult<School> GetSchool(Guid id)
    {
        var student = _service.GetById(id);

        if (student is null || student.School is null)
            return NotFound();

        return Ok(new {
            school = student.School
        });
    }
}
