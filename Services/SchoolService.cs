using Microsoft.EntityFrameworkCore;
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

    public IEnumerable<School> GetAll(){
        return _context.Schools
            .AsNoTracking()
            .ToList();
    }

    public School? GetById(Guid id){
        return _context.Schools
            .AsNoTracking()
            .SingleOrDefault(school => school.Id == id);
    }

    public void Create(School school)
    {
        _context.Schools
            .Add(school);

        _context.SaveChanges();
    }

    public void Create(School[] schools)
    {
        foreach(var school in schools)
        {
            _context.Schools
                .Add(school);
        }

        _context.SaveChanges();
    }

    public bool IdExists(Guid id)
    {
        var count = _context.Schools
            .Count(school => school.Id == id);

        return count > 0;
    }
}