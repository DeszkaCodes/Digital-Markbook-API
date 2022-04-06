using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Services;
using SchoolAPI.Models;

namespace SchoolAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ClassController
{
    private ClassService _service;

    public ClassController(ClassService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Class>> GetAll(int? limit)
    {
        if(limit is not null && limit > 0)
            return _service.GetAll().Take(limit.Value).ToArray();

        return _service.GetAll().ToArray();
    }

    [HttpGet("{id}")]
    public ActionResult<Class> GetById(Guid id)
    {
        return _service.GetById(id);
    }
}
