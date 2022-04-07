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

    public StudentController(StudentService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<Student>> GetMany(int? limit)
    {
        if(limit is null)
            return _service.GetAll().ToList();
            
        if(limit <= 0)
            return BadRequest(new { error = "Limit must be greater than 0" });

        return _service.GetAll().Take(limit.Value).ToList();
    }

    [HttpGet("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<Student> Get([FromRoute] Guid id){
        var student = _service.GetById(id);

        if(student is null)
            return NotFound(new { error = "Student not found" });

        return student;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public ActionResult<Student> Create(
        [FromForm] string name, [FromForm] string dateofbirth, [FromForm] Gender gender,
        [FromForm] Guid classId,
        [FromServices] SchoolService schoolService, [FromServices] SchoolClassService classService)
    {
        var schoolClass = classService.GetById(classId);

        if(schoolClass is null)
            return NotFound(new { error = "School Class not found" });

        var date = DateTime.Now;

        if(!DateTime.TryParse(dateofbirth, out date))
            return BadRequest(new { error = "Date of birth is not valid" });

        //TODO: Validate date of birth

        var student = new Student()
            {
                Id = Guid.NewGuid(),
                Name = name.ToTitleCase(),
                Gender = gender,
                DateOfBirth = date,
                School = schoolClass.School,
                Class = schoolClass
            };

        _service.Create(student);
        
        return CreatedAtAction(nameof(Create), student);
    }

    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Delete([FromRoute] Guid id)
    {
        var student = _service.GetById(id);

        if(student is null)
            return NotFound(new { error = "Student not found" });

        _service.DeleteById(id);
        
        return Ok();
    }

    [HttpPatch("{id:Guid}/name")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult UpdateName([FromRoute] Guid id, [FromQuery] string name)
    {
        var student = _service.GetById(id);

        if(student is null)
            return NotFound(new { error = "Student not found" });

        _service.UpdateName(id, name);

        return Ok();
    }

    [HttpGet("{id:Guid}/name")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<string> GetName([FromRoute] Guid id)
    {
        var student = _service.GetById(id);

        if (student is null)
            return NotFound(new { error = "Student not found" });

        return Ok(new { name = student.Name });
    }

    [HttpPatch("{id:Guid}/gender")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult UpdateGender([FromRoute] Guid id, [FromQuery] Gender gender)
    {
        var student = _service.GetById(id);

        if(student is null)
            return NotFound(new { error = "Student not found" });

        _service.UpdateGender(id, gender);

        return Ok();
    }

    [HttpGet("{id:Guid}/gender")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<Gender> GetGender([FromRoute] Guid id)
    {
        var student = _service.GetById(id);

        if(student is null)
            return NotFound(new { error = "Student not fount" });

        return Ok(new { gender = student.Gender });
    }

    [HttpPatch("{id:Guid}/dateofbirth")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult UpdateDateOfBirth([FromRoute] Guid id, [FromQuery] string dateofbirth)
    {
        var date = DateTime.Now;

        if(!DateTime.TryParse(dateofbirth, out date))
            return BadRequest(new { error = "Date of birth is not valid" });

        var student = _service.GetById(id);

        if(student is null)
            return NotFound(new { error = "Student not found" });

        //TODO: Validate date of birth

        _service.UpdateBirthDate(id, date);

        return Ok();
    }

    [HttpGet("{id:Guid}/dateofbirth")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<DateTime> GetDateOfBirth([FromRoute] Guid id)
    {
        var student = _service.GetById(id);

        if (student is null)
            return NotFound(new { error = "Student not found" });

        return Ok(new { dateofbirth = student.DateOfBirth });
    }

    [HttpPatch("{id:Guid}/school")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult UpdateSchool(
        [FromRoute] Guid id, [FromQuery] Guid schoolId,
        [FromServices] SchoolService schoolService)
    {
        var school = schoolService.GetById(schoolId);

        if(school is null)
            return NotFound(new { error = "School not found" });

        var student = _service.GetById(id);

        if(student is null)
            return NotFound(new { error = "Student not found" });

        _service.UpdateSchool(id, schoolId);

        return Ok();
    }

    [HttpGet("{id:Guid}/school")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<School> GetSchool([FromRoute] Guid id)
    {
        var student = _service.GetById(id);

        if (student is null)
            return NotFound(new { error = "Student not found" });

        return Ok(new { school = student.School });
    }
}
