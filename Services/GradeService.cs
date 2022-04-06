using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Models;

namespace SchoolAPI.Services;

public class GradeService : IService<Grade>
{
    private readonly SchoolContext _context;

    public GradeService(SchoolContext context)
    {
        _context = context;
    }

    public void Create(Grade model)
    {
        if(model.Subject is null)
            throw new ArgumentNullException("Subject cannot be null");

        if(model.Student is null)
            throw new ArgumentNullException("Student cannot be null");

        _context.Grades.Add(model);
        _context.Students.Attach(model.Student);
        _context.Subjects.Attach(model.Subject);

        _context.SaveChanges();
    }

    public void Create(Grade[] model)
    {
        for(int i = 0; i < model.Length; i++)
        {
            Create(model[i]);
        }
    }

    public void DeleteById(Guid id)
    {
        var grade = _context.Grades.Find(id);

        if(grade is null)
            throw new NullReferenceException("Grade not found");

        _context.Grades.Remove(grade);

        _context.SaveChanges();
    }

    public IEnumerable<Grade> GetAll()
    {
    #nullable disable
        return _context.Grades
            .Include(grade => grade.Subject)
            .Include(grade => grade.Subject.Class)
            .Include(grade => grade.Subject.Class.School)
            .Include(grade => grade.Subject.Class.HeadMaster)
            .Include(grade => grade.Subject.Class.HeadMaster.School)
            .Include(grade => grade.Student)
            .Include(grade => grade.Student.School)
            .Include(grade => grade.Student.Class)
            .Include(grade => grade.Student.Class.School)
            .Include(grade => grade.Student.Class.HeadMaster)
            .Include(grade => grade.Student.Class.HeadMaster.School)
            .AsNoTracking();
    #nullable enable
    }

    public Grade? GetById(Guid id)
    {
#nullable disable
        return _context.Grades
            .Include(grade => grade.Subject)
            .Include(grade => grade.Subject.Class)
            .Include(grade => grade.Subject.Class.School)
            .Include(grade => grade.Subject.Class.HeadMaster)
            .Include(grade => grade.Subject.Class.HeadMaster.School)
            .Include(grade => grade.Student)
            .Include(grade => grade.Student.School)
            .Include(grade => grade.Student.Class)
            .Include(grade => grade.Student.Class.School)
            .Include(grade => grade.Student.Class.HeadMaster)
            .Include(grade => grade.Student.Class.HeadMaster.School)
            .AsNoTracking()
            .SingleOrDefault(grade => grade.Id == id);
    #nullable enable
    }

    public void UpdateGrade(Guid id, byte gradeValue)
    {
        var grade = _context.Grades.Find(id);

        if(grade is null)
            throw new NullReferenceException("Grade not found");

        if(gradeValue < 1 || gradeValue > 5)
            throw new ArgumentOutOfRangeException("Grade must be between 1 and 5");

        if(grade.Value == gradeValue)
            return;

        grade.Value = gradeValue;

        _context.SaveChanges();
    }

    public bool IdExists(Guid id)
    {
        return _context.Grades.Find(id) is not null;
    }
}
