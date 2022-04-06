using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Services;
using SchoolAPI.Models;

namespace SchoolAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class SchoolClassController
{
    private SchoolClassService _service;

    public SchoolClassController(SchoolClassService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<IEnumerable<SchoolClass>> GetAll(int? limit)
    {
        if(limit is not null && limit > 0)
            return _service.GetAll().Take(limit.Value).ToArray();

        return _service.GetAll().ToArray();
    }

    [HttpGet("{id}")]
    public ActionResult<SchoolClass> GetById(Guid id)
    {
        return _service.GetById(id);
    }
}
