using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Models;
using SchoolAPI.Services;

namespace SchoolAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class GradeController : ControllerBase
{
    private readonly GradeService _service;
    private readonly SubjectService _subjectService;
    private readonly StudentService _studentService;


    public GradeController(GradeService service, SubjectService subjectService, StudentService studentService)
    {
        _service = service;
        _subjectService = subjectService;
        _studentService = studentService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Grade>> GetAll([FromQuery] int? limit)
    {
        if(limit is not null || limit > 0)
            return _service.GetAll().Take(limit.Value).ToList();

        return _service.GetAll().ToList();
    }

    [HttpGet("{id:Guid}")]
    public ActionResult<Grade> GetById([FromRoute] Guid id)
    {
        var grade = _service.GetById(id);

        if(grade is null)
            return NotFound(new { error = "Grade not found" });

        return grade;
    }

    [HttpPost]
    public ActionResult<Grade> Create(
        [FromForm(Name = "grade")] byte gradeValue, [FromForm] Guid subjectId, [FromForm] Guid studentId)
    {
        if(gradeValue < 1 || gradeValue > 5)
            return BadRequest(new { error = "Grade must be between 1 and 5" });

        var subject = _subjectService.GetById(studentId);

        if(subject is null)
            return NotFound(new { error = "Subject not found" });

        var student = _studentService.GetById(studentId);

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
    public IActionResult Delete([FromRoute] Guid id)
    {
        var grade = _service.GetById(id);

        if(grade is null)
            return NotFound(new { error = "Grade does not exist" });

        _service.DeleteById(id);

        return Ok();
    }

    [HttpPatch("{id:Guid}/grade")]
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
    public ActionResult GetGrade([FromRoute] Guid id)
    {
        var grade = _service.GetById(id);

        if(grade is null)
            return NotFound(new { error = "Grade not found" });

        return Ok(new
        {
            grade = grade.Value
        });
    }

    [HttpGet("{id:Guid}/subject")]
    public ActionResult GetSubject([FromRoute] Guid id)
    {
        var grade = _service.GetById(id);

        if(grade is null)
            return NotFound(new { error = "Grade not found" });

        return Ok(new {
            subject = grade.Subject
        });
    }

    [HttpGet("{id:Guid}/student")]
    public ActionResult GetStudent([FromRoute] Guid id)
    {
        var grade = _service.GetById(id);

        if(grade is null)
            return NotFound(new { error = "Grade not found" });

        return Ok(new {
            student = grade.Student
        });
    }
}
