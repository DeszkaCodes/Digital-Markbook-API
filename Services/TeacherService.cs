using Microsoft.EntityFrameworkCore;
using SchoolAPI.Models;
using SchoolAPI.Data;
using SchoolAPI.Classes;

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
            .Include(teacher => teacher.School)
            .AsNoTracking()
            .ToList();
    }

    public Teacher? GetById(Guid id){
        return _context.Teachers
            .Include(teacher => teacher.School)
            .AsNoTracking()
            .SingleOrDefault(teacher => teacher.Id == id);
    }

    public void Create(Teacher teacher)
    {
        _context.Teachers
            .Add(teacher);

        if(teacher.School is null)
            throw new ArgumentNullException("School cannot be null");

        _context.Schools
            .Attach(teacher.School);

        _context.SaveChanges();
    }

    public void Create(Teacher[] teachers)
    {
        foreach(var teacher in teachers)
        {
            _context.Teachers
                .Add(teacher);

            if (teacher.School is not null)
                _context.Schools.Attach(teacher.School);
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

    public void UpdateName(Guid id, string name)
    {
        var teacher = _context.Teachers.Find(id);

        if(teacher is null)
            throw new NullReferenceException("Teacher does not exist");

        teacher.Name = name;

        _context.SaveChanges();
    }

    public void UpdateGender(Guid id, Gender gender)
    {
        var teacher = _context.Teachers.Find(id);

        if (teacher is null)
            throw new NullReferenceException("Teacher does not exist");

        teacher.Gender = gender;

        _context.SaveChanges();
    }

    public void UpdateBirthDate(Guid id, DateTime newDate)
    {
        var teacher = _context.Teachers.Find(id);

        if (teacher is null)
            throw new NullReferenceException("Teacher does not exist");

        if(newDate > DateTime.Now || newDate > DateTime.Now.AddYears(-25))
            throw new ArgumentException("The gived date is not possible");

        teacher.DateOfBirth = newDate;

        _context.SaveChanges();
    }

    public void UpdateSchool(Guid id, Guid schoolId)
    {
        var teacher = _context.Teachers.Find(id);
        var school = _context.Schools.Find(schoolId);

        if (teacher is null)
            throw new NullReferenceException("Teacher does not exist");
        if(school is null)
            throw new NullReferenceException("School does not exist");

        teacher.School = school;

        _context.SaveChanges();
    }
}