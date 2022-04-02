using Microsoft.EntityFrameworkCore;
using SchoolAPI.Classes;
using SchoolAPI.Models;
using SchoolAPI.Data;

namespace SchoolAPI.Services;

public class StudentService : IService<Student>
{
    private readonly SchoolContext _context;

    public StudentService(SchoolContext context)
    {
        _context = context;
    }

    public IEnumerable<Student> GetAll()
    {
        #nullable disable
        return _context.Students
            .Include(student => student.School)
            .Include(student => student.Class)
            .Include(student => student.Class.HeadMaster)
            .Include(student => student.Class.HeadMaster.School)
            .Include(student => student.Class.School)
            .AsNoTracking()
            .ToList();
        #nullable enable
    }

    public Student? GetById(Guid id)
    {
        #nullable disable
        return _context.Students
            .Include(student => student.School)
            .Include(student => student.Class)
            .Include(student => student.Class.HeadMaster)
            .Include(student => student.Class.HeadMaster.School)
            .Include(student => student.Class.School)
            .AsNoTracking()
            .SingleOrDefault(student => student.Id == id);
        #nullable enable
    }

    public void Create(Student[] students)
    {
        foreach(var student in students)
        {
            _context.Students
                .Add(student);
        }

        _context.SaveChanges();
    }

    public void Create(Student student)
    {
        _context.Students 
            .Add(student);

        if(student.School is null)
            throw new ArgumentNullException("School cannot be null");

        if(student.Class is null)
            throw new ArgumentNullException("Class cannot be null");

        _context.Schools
            .Attach(student.School);

        _context.Classes
            .Attach(student.Class);

        _context.SaveChanges();
    }

    public void DeleteById(Guid id)
    {
        var toDelete = _context.Students.Find(id);
        if(toDelete is not null)
        {
            _context.Students.Remove(toDelete);
            _context.SaveChanges();
        }
    }

    public void UpdateName(Guid id, string newName)
    {
        var student = _context.Students.Find(id);

        if(student is null)
            throw new NullReferenceException("Student does not exist");

        student.Name = newName;

        _context.SaveChanges();
    }

    public void UpdateGender(Guid id, Gender newGender)
    {
        var student = _context.Students.Find(id);

        if(student is null)
            throw new NullReferenceException("Student does not exist");

        student.Gender = newGender;

        _context.SaveChanges();
    }

    public void UpdateBirthDate(Guid id, DateTime newDate)
    {
        var student = _context.Students.Find(id);

        if(student is null)
            throw new NullReferenceException("Student does not exist");

        if(newDate > DateTime.Now || newDate < DateTime.Now.AddYears(-6))
            throw new ArgumentException("The gived date is not possible");

        student.DateOfBirth = newDate;

        _context.SaveChanges();
    }

    public void UpdateSchool(Guid id, Guid schoolId)
    {
        var student = _context.Students.Find(id);
        var newSchool = _context.Schools.Find(schoolId);

        if(student is null)
            throw new NullReferenceException("Student does not exist");

        if(newSchool is null)
            throw new NullReferenceException("School does not exist");

        if(newSchool.Id == student.School?.Id)
            throw new ArgumentException("New school is the current school");

        student.School = newSchool;

        _context.SaveChanges();
    }

    //TODO: update class

    public bool IdExists(Guid id)
    {
        var count = _context.Students
            .Count(student => student.Id == id);

        return count > 0;
    }
}