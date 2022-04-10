using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Models;
using SchoolAPI.Services;

namespace SchoolAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class GradeReaders : ControllerBase
{
    private readonly GradeService _service;

    public GradeReaders(GradeService service)
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
