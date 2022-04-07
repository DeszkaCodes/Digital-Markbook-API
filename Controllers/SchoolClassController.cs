using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Services;
using SchoolAPI.Models;

namespace SchoolAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class SchoolClassController : ControllerBase
{
    private SchoolClassService _service;

    public SchoolClassController(SchoolClassService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<SchoolClass>> GetAll([FromQuery] int? limit)
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
    public ActionResult<SchoolClass> GetById([FromRoute] Guid id)
    {
        var schoolClass = _service.GetById(id);

        if(schoolClass is null)
            return NotFound(new { error = "School Class not found" });

        return schoolClass;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public ActionResult<SchoolClass> Create(
        [FromForm] string name, [FromForm] Guid headMasterId, [FromForm] Guid schoolId,
        [FromServices] TeacherService teacherService, [FromServices] SchoolService schoolService)
    {
        var headMaster = teacherService.GetById(headMasterId);

        if(headMaster is null)
            return NotFound(new { error = "Teacher does not exist" });

        var school = schoolService.GetById(schoolId);

        if(school is null)
            return NotFound(new { error = "School does not exist" });

        var schoolClass = new SchoolClass()
        {
            Id = Guid.NewGuid(),
            Name = name,
            HeadMaster = headMaster,
            School = school
        };

        _service.Create(schoolClass);

        return CreatedAtAction(nameof(Create), schoolClass);
    }

    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Delete([FromRoute] Guid id)
    {
        var schoolClass = _service.GetById(id);

        if(schoolClass is null)
            return NotFound(new { error = "School class does not exist" });

        _service.DeleteById(id);

        return Ok();
    }

    [HttpPatch("{id:Guid}/name")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult UpdateName([FromRoute] Guid id, [FromQuery] string name)
    {
        var schoolClass = _service.GetById(id);

        if(schoolClass is null)
            return NotFound(new { error = "School class does not exist" });

        _service.UpdateName(schoolClass.Id, name);

        return Ok();
    }

    [HttpGet("{id:Guid}/name")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult GetName([FromRoute] Guid id)
    {
        var schoolClass = _service.GetById(id);

        if(schoolClass is null)
            return NotFound(new { error = "School class does not exist" });

        return Ok(new { name = schoolClass.Name });
    }

    [HttpPatch("{id:Guid}/headmaster")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult UpdateHeadMaster(
        [FromRoute] Guid id, [FromQuery] Guid headMasterId,
        [FromServices] TeacherService teacherService)
    {
        var schoolClass = _service.GetById(id);

        if(schoolClass is null)
            return NotFound(new { error = "School class does not exist" });

        var headMaster = teacherService.GetById(headMasterId);

        if(headMaster is null)
            return NotFound(new { error = "Teacher does not exist" });

        _service.UpdateHeadmaster(schoolClass.Id, headMasterId);

        return Ok();
    }

    [HttpGet("{id:Guid}/headmaster")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<Teacher> GetHeadMaster([FromRoute] Guid id)
    {
        var schoolClass = _service.GetById(id);

        if(schoolClass is null)
            return NotFound(new { error = "School class does not exist" });

        if(schoolClass.HeadMaster is null)
            return NotFound(new { error = "School class does not have a headmaster" });

        return Ok(new { headmaster = schoolClass.HeadMaster });
    }

    [HttpPatch("{id:Guid}/school")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult UpdateSchool(
        [FromRoute] Guid id, [FromQuery] Guid schoolId,
        [FromServices] SchoolService schoolService)
    {
        var schoolClass = _service.GetById(id);

        if(schoolClass is null)
            return NotFound(new { error = "School class does not exist" });

        var school = schoolService.GetById(schoolId);

        if(school is null)
            return NotFound(new { error = "School does not exist" });

        _service.UpdateSchool(schoolClass.Id, schoolId);

        return Ok();
    }

    [HttpGet("{id:Guid}/school")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<School> GetSchool([FromRoute] Guid id)
    {
        var schoolClass = _service.GetById(id);

        if(schoolClass is null)
            return NotFound(new { error = "School class does not exist" });

        if(schoolClass.School is null)
            return NotFound(new { error = "School class does not have a school" });

        return Ok(new { school = schoolClass.School });
    }
}
