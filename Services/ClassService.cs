using SchoolAPI.Data;
using SchoolAPI.Models;

namespace SchoolAPI.Services;

public class ClassService : IService<Class>
{
    private readonly SchoolContext _context;

    public ClassService(SchoolContext context)
    {
        _context = context;
    }

    public void Create(Class model)
    {
        throw new NotImplementedException();
    }

    public void Create(Class[] model)
    {
        throw new NotImplementedException();
    }

    public void DeleteById(Guid id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Class> GetAll()
    {
        throw new NotImplementedException();
    }

    public Class? GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public bool IdExists(Guid id)
    {
        throw new NotImplementedException();
    }
}
