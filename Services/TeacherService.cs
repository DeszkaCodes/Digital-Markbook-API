using Microsoft.EntityFrameworkCore;
using SchoolAPI.Models;
using SchoolAPI.Data;

namespace SchoolAPI.Services;

public class TeacherService
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

    public Teacher[] Create(Teacher[] teachers)
    {
        foreach(var teacher in teachers)
        {
            _context.Teachers
                .Add(teacher);
        }

        _context.SaveChanges();

        return teachers;
    }
}