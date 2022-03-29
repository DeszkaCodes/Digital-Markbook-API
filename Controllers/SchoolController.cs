using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Models;
using SchoolAPI.Services;
using SchoolAPI.Generators;

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
    public ActionResult<School> Get(Guid id){
        var school = _service.GetById(id);

        if(school is null)
            return NotFound();

        return school;
    }
}