using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Classes;
using SchoolAPI.Models;
using SchoolAPI.Services;

namespace SchoolAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class SchoolModifiers : ControllerBase
{
    private readonly SchoolService _service;

    public SchoolModifiers(SchoolService service)
    {
        _service = service;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public ActionResult<School> Create([FromForm] string name, [FromForm] SchoolType type)
    {
        var school = new School()
        {
            Id = Guid.NewGuid(),
            Name = name,
            Type = type
        };

        _service.Create(school);

        return CreatedAtAction(nameof(Create), school);
    }

    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Delete([FromRoute] Guid id)
    {
        var school = _service.GetById(id);

        if(school is null)
            return NotFound(new { error = "School not found" });

        _service.DeleteById(id);

        return Ok();
    }

    [HttpPatch("{id:Guid}/name")]
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

    [HttpPatch("{id:Guid}/type")]
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
}
