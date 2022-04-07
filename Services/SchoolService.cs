using Microsoft.EntityFrameworkCore;
using SchoolAPI.Classes;
using SchoolAPI.Models;
using SchoolAPI.Data;

namespace SchoolAPI.Services;

public class SchoolService : IService<School>
{
    private readonly SchoolContext _context;

    public SchoolService(SchoolContext context)
    {
        _context = context;
    }

    public IEnumerable<School> GetAll()
    {
        return _context.Schools
            .AsNoTracking()
            .ToList();
    }

    public IEnumerable<Teacher> GetTeachersById(Guid id)
    {
        return _context.Teachers
            .Include(teacher => teacher.School)
            .Where(teacher => teacher.School != null && teacher.School.Id == id)
            .AsNoTracking();
    }

    public School? GetById(Guid id){
        return _context.Schools
            .AsNoTracking()
            .SingleOrDefault(school => school.Id == id);
    }

    public void Create(School school)
    {
        if(school.Name is null)
            throw new ArgumentNullException("School name cannot be null");

        school.Name.ToTitleCase();
        
        _context.Schools
            .Add(school);
        
        _context.SaveChanges();
    }

    public void Create(School[] schools)
    {
        for(var i = 0; i < schools.Length; i++)
        {
            Create(schools[i]);
        }
    }

    public void DeleteById(Guid id)
    {
        var toDelete = _context.Schools.Find(id);
        if(toDelete is not null)
        {
            _context.Schools.Remove(toDelete);
            _context.SaveChanges();
        }
    }

    public bool IdExists(Guid id)
    {
        var count = _context.Schools
            .Count(school => school.Id == id);

        return count > 0;
    }

    public void UpdateName(Guid id, string name)
    {
        var school = _context.Schools.Find(id);

        if(school is null)
            throw new NullReferenceException("School does not exist");

        school.Name = name.ToTitleCase();

        _context.SaveChanges();
    }

    public void UpdateType(Guid id, SchoolType type)
    {
        var school = _context.Schools.Find(id);

        if(school is null)
            throw new NullReferenceException("School does not exist");

        school.Type = type;

        _context.SaveChanges();
    }
}