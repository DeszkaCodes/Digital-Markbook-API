using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Services;
using SchoolAPI.Classes;
using SchoolAPI.Models;

namespace SchoolAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class SchoolController : ControllerBase
{
    private SchoolService _service;

    public SchoolController(SchoolService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<School>> GetAll([FromQuery] int? limit) 
    {
        if(limit is null)
            return _service.GetAll().ToList();
            
        if(limit <= 0)
            return BadRequest(new { error = "Limit must be greater than 0" });

        return _service.GetAll().Take(limit.Value).ToList();
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<School> Get([FromRoute] Guid id)
    {
        var school = _service.GetById(id);

        if(school is null)
            return NotFound(new { error = "School not found" });

        return school;
    }

    [HttpGet("{id}/teachers")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<Teacher>> GetAllTeachers([FromRoute] Guid id)
    {
        var school = _service.GetById(id);

        if (school is null)
            return NotFound(new { error = "School not found" });

        return _service.GetTeachersById(id).ToList();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public ActionResult<School> Create([FromForm] string name, [FromForm] SchoolType type)
    {
        var school = new School()
            {
                Id = Guid.NewGuid(),
                Name = name.ToTitleCase(),
                Type = type
            };

        _service.Create(school);

        return CreatedAtAction(nameof(Create), school);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Delete([FromQuery] Guid id)
    {
        var school = _service.GetById(id);

        if(school is null)
            return NotFound(new { error = "School not found" });

        _service.DeleteById(id);

        return Ok();
    }

    [HttpPatch("{id}/name")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult UpdateName([FromRoute] Guid id, [FromQuery] string name)
    {
        var school = _service.GetById(id);

        if(school is null)
            return NotFound(new { error = "School not found" });

        _service.UpdateName(id, name);

        return Ok();
    }

    [HttpGet("{id}/name")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<string> GetName([FromRoute] Guid id)
    {
        var school = _service.GetById(id);

        if(school is null)
            return NotFound(new { error = "School not found" });

        return Ok(new { name = school.Name });
    }

    [HttpPatch("{id}/type")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult UpdateType([FromRoute] Guid id, [FromQuery] SchoolType type)
    {
        var school = _service.GetById(id);

        if(school is null)
            return NotFound(new { error = "School not found" });

        _service.UpdateType(id, type);

        return Ok();
    }

    [HttpGet("{id}/type")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<SchoolType> GetType([FromRoute] Guid id)
    {
        var school = _service.GetById(id);

        if (school is null)
            return NotFound(new { error = "School not found" });

        return Ok(new { type = school.Type });
    }
}
