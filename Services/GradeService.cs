using SchoolAPI.Data;
using SchoolAPI.Models;

namespace SchoolAPI.Services;

public class MarkService : IService<Grade>
{
    private readonly SchoolContext _context;

    public MarkService(SchoolContext context)
    {
        _context = context;
    }

    public void Create(Grade model)
    {
        throw new NotImplementedException();
    }

    public void Create(Grade[] model)
    {
        throw new NotImplementedException();
    }

    public void DeleteById(Guid id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Grade> GetAll()
    {
        throw new NotImplementedException();
    }

    public Grade? GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public bool IdExists(Guid id)
    {
        throw new NotImplementedException();
    }
}
