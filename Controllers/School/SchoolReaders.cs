using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Services;
using SchoolAPI.Classes;
using SchoolAPI.Models;

namespace SchoolAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class SchoolReaders : ControllerBase
{
    private SchoolService _service;

    public SchoolReaders(SchoolService service)
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
    
    [HttpGet("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<School> Get([FromRoute] Guid id)
    {
        var school = _service.GetById(id);

        if(school is null)
            return NotFound(new { error = "School not found" });

        return school;
    }

    [HttpGet("{id:Guid}/teachers")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<Teacher>> GetAllTeachers([FromRoute] Guid id)
    {
        var school = _service.GetById(id);

        if (school is null)
            return NotFound(new { error = "School not found" });

        return _service.GetTeachersById(id).ToList();
    }

    [HttpGet("{id:Guid}/name")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<string> GetName([FromRoute] Guid id)
    {
        var school = _service.GetById(id);

        if(school is null)
            return NotFound(new { error = "School not found" });

        return Ok(new { name = school.Name });
    }

    [HttpGet("{id:Guid}/type")]
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
