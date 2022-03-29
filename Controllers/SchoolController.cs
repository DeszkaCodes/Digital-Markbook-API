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
    public ActionResult<List<School>> GetAll(int? limit) 
    {
        if(limit is not null && limit > 0)
            return _service.GetAll().Take((int)limit).ToList();

        return _service.GetAll().ToList();
    }
    
    [HttpGet("{id}")]
    public ActionResult<School> Get(Guid id)
    {
        var school = _service.GetById(id);

        if(school is null)
            return NotFound();

        return school;
    }

    [HttpPost]
    public ActionResult<School> Create(string name, SchoolType type)
    {
        Guid id;

        do
        {
            id = Guid.NewGuid();
        } while(_service.IdExists(id));

        var school = new School()
            {
                Name = name.ToTitleCase(),
                Type = type
            };

        _service.Create(school);

        return school;
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var school = _service.GetById(id);

        if(school is null)
            return NotFound();

        _service.DeleteById(id);

        return Ok();
    }

    [HttpPut("{id}/name")]
    public IActionResult UpdateName(Guid id, string name)
    {
        if(_service.GetById(id) is null)
            return NotFound();

        _service.UpdateName(id, name);

        return Ok();
    }

    [HttpPut("{id}/type")]
    public IActionResult UpdateType(Guid id, SchoolType type)
    {
        if(_service.GetById(id) is null)
            return NotFound();

        _service.UpdateType(id, type);

        return Ok();
    }
}