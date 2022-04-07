using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Models;
using SchoolAPI.Services;
using SchoolAPI.Classes;

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
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<Teacher>> GetAll([FromQuery] int? limit)
    {
        if (limit is null)
            return _service.GetAll().ToList();
            
        if(limit <= 0)
            return BadRequest(new { error = "Limit must be greater than 0" });

        return _service.GetAll().Take(limit.Value).ToList();
    }

    [HttpGet("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<Teacher> Get([FromRoute] Guid id)
    {
        var teacher = _service.GetById(id);

        if (teacher is null)
            return NotFound(new { error = "Teacher not found" });

        return teacher;
    }

    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Delete([FromRoute] Guid id)
    {
        var teacher = _service.GetById(id);
        
        if (teacher is null)
            return NotFound(new { error = "Teacher not found" });

        _service.DeleteById(id);

        return Ok();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public ActionResult<Teacher> Create(
        [FromForm] string name, [FromForm] string dateOfBirth, [FromForm] Gender gender, [FromForm] Guid schoolId,
        [FromServices] SchoolService schoolService)
    {
        var school = schoolService.GetById(schoolId);

        if(school is null)
            return NotFound(new { error = "School not found" });
        
        var date = DateTime.Now;

        if (!DateTime.TryParse(dateOfBirth, out date))
            return BadRequest(new { error = "Date of birth is not valid" });

        if(date > DateTime.Today.AddYears(Teacher.MaximumAge))
            return BadRequest(new { error = "Teacher is too old" });

        if(date < DateTime.Today.AddYears(-Teacher.MaximumAge))
            return BadRequest(new { error = "Teacher is too young" });

        var teacher = new Teacher()
        {
            Id = Guid.NewGuid(),
            Name = name,
            DateOfBirth = date,
            Gender = gender,
            School = school
        };

        _service.Create(teacher);

        return CreatedAtAction(nameof(Create), teacher);
    }

    [HttpPatch("{id:Guid}/name")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult UpdateName([FromRoute] Guid id, [FromQuery] string name)
    {
        var teacher = _service.GetById(id);

        if(teacher is null)
            return NotFound(new { error = "Teacher not found" });

        _service.UpdateName(id, name);

        return Ok();
    }

    [HttpGet("{id:Guid}/name")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<string> GetName([FromRoute] Guid id)
    {
        var teacher = _service.GetById(id);

        if (teacher is null)
            return NotFound(new { error = "Teacher not found" });

        return Ok(new { name = teacher.Name });
    }   

    [HttpPatch("{id:Guid}/gender")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult UpdateGender([FromRoute] Guid id, [FromQuery] Gender gender)
    {
        var teacher = _service.GetById(id);

        if(teacher is null)
            return NotFound(new { error = "Teacher not found" });

        _service.UpdateGender(id, gender);

        return Ok();
    }

    [HttpGet("{id:Guid}/gender")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<Gender> GetGender([FromRoute] Guid id)
    {
        var teacher = _service.GetById(id);

        if(teacher is null)
            return NotFound(new { error = "Teacher not found" });

        return Ok(new { gender = teacher.Gender });
    }


    [HttpPatch("{id:Guid}/dateofbirth")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult UpdateDateOfBirth([FromRoute] Guid id, [FromQuery] string birthDate)
    {
        var teacher = _service.GetById(id);

        if(teacher is null)
            return NotFound(new { error = "Teacher not found" });

        var date = DateTime.Now;

        if(!DateTime.TryParse(birthDate, out date))
            return BadRequest(new { error = "Date of birth is not valid" });

        //TODO: Validate date of birth

        _service.UpdateBirthDate(id, date);

        return Ok();
    }

    [HttpGet("{id:Guid}/dateofbirth")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<DateTime> GetDateOfBirth([FromRoute] Guid id)
    {
        var teacher = _service.GetById(id);

        if (teacher is null)
            return NotFound(new { error = "Teacher not found" });

        return Ok(new { dateofbirth = teacher.DateOfBirth });
    }

    [HttpPatch("{id:Guid}/school")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult UpdateSchool(
        [FromRoute] Guid id, [FromQuery] Guid schoolId,
        [FromServices] SchoolService schoolService)
    {
        var teacher = _service.GetById(id);

        if (teacher is null)
            return NotFound(new { error = "Teacher not found" });
        
        var school = schoolService.GetById(schoolId);

        if(school is null)
            return NotFound(new { error = "School not found" });

        _service.UpdateSchool(id, schoolId);

        return Ok();
    }

    [HttpGet("{id:Guid}/school")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<School> GetSchool([FromRoute] Guid id)
    {
        var teacher = _service.GetById(id);

        if(teacher is null)
            return NotFound(new { error = "Teacher not found" });

        return Ok(new { school = teacher.School });
    }
}
