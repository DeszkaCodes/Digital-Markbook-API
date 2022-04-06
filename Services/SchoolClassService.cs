using SchoolAPI.Data;
using SchoolAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace SchoolAPI.Services;

public class SchoolClassService : IService<SchoolClass>
{
    private readonly SchoolContext _context;

    public SchoolClassService(SchoolContext context)
    {
        _context = context;
    }

    public void Create(SchoolClass model)
    {
        if(model.Name is null)
            throw new ArgumentNullException("Name cannot be null");

        if(model.HeadMaster is null)
            throw new ArgumentNullException("HeadMaster cannot be null");

        if(model.School is null)
            throw new ArgumentNullException("School cannot be null");

        _context.Classes.Add(model);

        _context.SaveChanges();
    }

    public void Create(SchoolClass[] model)
    {
        for(int i = 0; i < model.Length; i++)
        {
            Create(model[i]);
        }
    }

    public void DeleteById(Guid id)
    {
        var schoolClass = _context.Classes.Find(id);

        if(schoolClass is null)
            throw new Exception("School class not found");

        _context.Classes.Remove(schoolClass);

        _context.SaveChanges();
    }

    public IEnumerable<SchoolClass> GetAll()
    {
        #nullable disable
        return _context.Classes
            .Include(c => c.School)
            .Include(c => c.HeadMaster)
            .Include(c => c.HeadMaster.School)
            .AsNoTracking();
        #nullable enable
    }

    public SchoolClass? GetById(Guid id)
    {
        #nullable disable
        return _context.Classes
            .Include(c => c.School)
            .Include(c => c.HeadMaster)
            .Include(c => c.HeadMaster.School)
            .AsNoTracking()
            .FirstOrDefault(c => c.Id == id);
        #nullable enable
    }

    public bool IdExists(Guid id)
    {
        return _context.Classes.Find(id) is not null;
    }

    public void UpdateName(Guid id, string name)
    {
        var schoolClass = _context.Classes.Find(id);

        if(schoolClass is null)
            throw new NullReferenceException("School class does not exist");

        schoolClass.Name = name;

        _context.SaveChanges();
    }

    public void UpdateHeadmaster(Guid id, Guid teacherId)
    {
        var schoolClass = _context.Classes.Find(id);

        if(schoolClass is null)
            throw new NullReferenceException("School class does not exist");

        var teacher = _context.Teachers.Find(teacherId);
        
        if(teacher is null)
            throw new NullReferenceException("Teacher does not exist");

        schoolClass.HeadMaster = teacher;

        _context.SaveChanges();
    }

    public void UpdateSchool(Guid id, Guid schoolId)
    {
        var schoolClass = _context.Classes.Find(id);

        if(schoolClass is null)
            throw new NullReferenceException("School class does not exist");

        var school = _context.Schools.Find(schoolId);
        
        if(school is null)
            throw new NullReferenceException("School does not exist");

        schoolClass.School = school;

        _context.SaveChanges();
    }
}
