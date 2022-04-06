using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Services;
using SchoolAPI.Models;
using SchoolAPI.Classes;

namespace SchoolAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class SchoolClassController : ControllerBase
{
    private SchoolClassService _service;
    private TeacherService _teacherService;
    private SchoolService _schoolService;

    public SchoolClassController(SchoolClassService service, TeacherService teacherService, SchoolService schoolService)
    {
        _service = service;
        _schoolService = schoolService;
        _teacherService = teacherService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<SchoolClass>> GetAll([FromQuery] int? limit)
    {
        if(limit is not null && limit > 0)
            return _service.GetAll().Take(limit.Value).ToArray();

        return _service.GetAll().ToArray();
    }

    [HttpGet("{id:Guid}")]
    public ActionResult<SchoolClass> GetById([FromRoute] Guid id)
    {
        var schoolClass = _service.GetById(id);

        if(schoolClass is null)
            return NotFound();

        return schoolClass;
    }

    [HttpPost]
    public ActionResult<SchoolClass> Create(string name, Guid headMasterId, Guid schoolId)
    {
        var headMaster = _teacherService.GetById(headMasterId);

        if(headMaster is null)
            return NotFound(new { error = "Teacher does not exist" });

        var school = _schoolService.GetById(schoolId);

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

        return schoolClass;
    }

    [HttpDelete("{id:Guid}")]
    public IActionResult Delete([FromRoute] Guid id)
    {
        var schoolClass = _service.GetById(id);

        if(schoolClass is null)
            return NotFound(new { error = "School class does not exist" });

        _service.DeleteById(id);

        return Ok();
    }

    [HttpPatch("{id:Guid}/name")]
    public IActionResult UpdateName([FromRoute] Guid id, [FromQuery] string name)
    {
        var schoolClass = _service.GetById(id);

        if(schoolClass is null)
            return NotFound(new { error = "School class does not exist" });

        _service.UpdateName(schoolClass.Id, name);

        return Ok();
    }

    [HttpGet("{id:Guid}/name")]
    public ActionResult GetName([FromRoute] Guid id)
    {
        var schoolClass = _service.GetById(id);

        if(schoolClass is null)
            return NotFound(new { error = "School class does not exist" });

        if(schoolClass.Name is null)
            return NotFound(new { error = "School class does not have a name" });

        return Ok(new { name = schoolClass.Name });
    }

    [HttpPatch("{id:Guid}/headmaster")]
    public IActionResult UpdateHeadMaster([FromRoute] Guid id, [FromQuery] Guid headMasterId)
    {
        var schoolClass = _service.GetById(id);

        if(schoolClass is null)
            return NotFound(new { error = "School class does not exist" });

        var headMaster = _teacherService.GetById(headMasterId);

        if(headMaster is null)
            return NotFound(new { error = "Teacher does not exist" });

        _service.UpdateHeadmaster(schoolClass.Id, headMasterId);

        return Ok();
    }

    [HttpGet("{id:Guid}/headmaster")]
    public ActionResult<Teacher> GetHeadMaster([FromRoute] Guid id)
    {
        var schoolClass = _service.GetById(id);

        if(schoolClass is null)
            return NotFound(new { error = "School class does not exist" });

        if(schoolClass.HeadMaster is null)
            return NotFound(new { error = "School class does not have a headmaster" });

        return schoolClass.HeadMaster;
    }

    [HttpPatch("{id:Guid}/school")]
    public IActionResult UpdateSchool([FromRoute] Guid id, [FromQuery] Guid schoolId)
    {
        var schoolClass = _service.GetById(id);

        if(schoolClass is null)
            return NotFound(new { error = "School class does not exist" });

        var school = _schoolService.GetById(schoolId);

        if(school is null)
            return NotFound(new { error = "School does not exist" });

        _service.UpdateSchool(schoolClass.Id, schoolId);

        return Ok();
    }

    [HttpGet("{id:Guid}/school")]
    public ActionResult<School> GetSchool([FromRoute] Guid id)
    {
        var schoolClass = _service.GetById(id);

        if(schoolClass is null)
            return NotFound(new { error = "School class does not exist" });

        if(schoolClass.School is null)
            return NotFound(new { error = "School class does not have a school" });

        return schoolClass.School;
    }
}
