using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Models;
using SchoolAPI.Services;

namespace SchoolAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class GradeController : ControllerBase
{
    private readonly GradeService _service;

    public GradeController(GradeService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<Grade>> GetAll([FromQuery] int? limit)
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
    public ActionResult<Grade> GetById([FromRoute] Guid id)
    {
        var grade = _service.GetById(id);

        if(grade is null)
            return NotFound(new { error = "Grade not found" });

        return grade;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public ActionResult<Grade> Create(
        [FromForm(Name = "grade")] byte gradeValue, [FromForm] Guid subjectId, [FromForm] Guid studentId,
        [FromServices] SubjectService subjectService, [FromServices] StudentService studentService)
    {
        if(gradeValue < 1 || gradeValue > 5)
            return BadRequest(new { error = "Grade must be between 1 and 5" });

        var subject = subjectService.GetById(studentId);

        if(subject is null)
            return NotFound(new { error = "Subject not found" });

        var student = studentService.GetById(studentId);

        if(student is null)
            return NotFound(new { error = "Student not found" });

        var grade = new Grade()
        {
            Id = Guid.NewGuid(),
            Value = gradeValue,
            Student = student,
            Subject = subject
        };

        _service.Create(grade);

        return CreatedAtAction(nameof(Created), grade);
    }

    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Delete([FromRoute] Guid id)
    {
        var grade = _service.GetById(id);

        if(grade is null)
            return NotFound(new { error = "Grade does not exist" });

        _service.DeleteById(id);

        return Ok();
    }

    [HttpPatch("{id:Guid}/grade")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult UpdateGrade([FromRoute] Guid id, [FromQuery(Name = "grade")] byte gradeValue)
    {
        var grade = _service.GetById(id);

        if(grade is null)
            return NotFound(new { error = "Grade not found" });

        if(gradeValue < 1 || gradeValue > 5)
            return BadRequest(new { error = "Grade must be between 1 and 5" });

        _service.UpdateGrade(id, gradeValue);

        return Ok();
    }

    [HttpGet("{id:Guid}/grade")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult GetGrade([FromRoute] Guid id)
    {
        var grade = _service.GetById(id);

        if(grade is null)
            return NotFound(new { error = "Grade not found" });

        return Ok(new { grade = grade.Value });
    }

    [HttpGet("{id:Guid}/subject")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult GetSubject([FromRoute] Guid id)
    {
        var grade = _service.GetById(id);

        if(grade is null)
            return NotFound(new { error = "Grade not found" });

        return Ok(new { subject = grade.Subject });
    }

    [HttpGet("{id:Guid}/student")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult GetStudent([FromRoute] Guid id)
    {
        var grade = _service.GetById(id);

        if(grade is null)
            return NotFound(new { error = "Grade not found" });

        return Ok(new { student = grade.Student });
    }
}
