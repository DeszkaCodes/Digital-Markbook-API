using SchoolAPI.Data;
using SchoolAPI.Models;

namespace SchoolAPI.Services;

public class SubjectService : IService<Subject>
{
    private readonly SchoolContext _context;

    public SubjectService(SchoolContext context)
    {
        _context = context;
    }

    public void Create(Subject model)
    {
        throw new NotImplementedException();
    }

    public void Create(Subject[] model)
    {
        throw new NotImplementedException();
    }

    public void DeleteById(Guid id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Subject> GetAll()
    {
        throw new NotImplementedException();
    }

    public Subject? GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public bool IdExists(Guid id)
    {
        throw new NotImplementedException();
    }
}
