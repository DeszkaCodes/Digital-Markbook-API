using Microsoft.EntityFrameworkCore;
using SchoolAPI.Models;
using SchoolAPI.Data;

namespace SchoolAPI.Services;

public class TeacherService : IService<Teacher>
{
    private readonly SchoolContext _context;

    public TeacherService(SchoolContext context)
    {
        _context = context;
    }

    public IEnumerable<Teacher> GetAll(){
        return _context.Teachers
            .AsNoTracking()
            .ToList();
    }

    public Teacher? GetById(Guid id){
        return _context.Teachers
            .AsNoTracking()
            .SingleOrDefault(teacher => teacher.Id == id);
    }

    public void Create(Teacher teacher)
    {
        _context.Teachers
            .Add(teacher);

        _context.SaveChanges();
    }

    public void Create(Teacher[] teachers)
    {
        foreach(var teacher in teachers)
        {
            _context.Teachers
                .Add(teacher);
        }

        _context.SaveChanges();
    }

    public void DeleteById(Guid id)
    {
        var toDelete = _context.Teachers.Find(id);
        if(toDelete is not null)
        {
            _context.Teachers.Remove(toDelete);
            _context.SaveChanges();
        }
    }

    public bool IdExists(Guid id)
    {
        var count = _context.Teachers
            .Count(teacher => teacher.Id == id);

        return count > 0;
    }
}