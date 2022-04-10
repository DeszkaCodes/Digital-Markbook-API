using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Models;
using SchoolAPI.Services;

namespace SchoolAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class GradeModifiers : ControllerBase
{
    private readonly GradeService _service;

    public GradeModifiers(GradeService service)
    {
        _service = service;
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
}
