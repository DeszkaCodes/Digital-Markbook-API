using SchoolAPI.Data;
using SchoolAPI.Models;

namespace SchoolAPI.Services;

public class MarkService : IService<Mark>
{
    private readonly SchoolContext _context;

    public MarkService(SchoolContext context)
    {
        _context = context;
    }

    public void Create(Mark model)
    {
        throw new NotImplementedException();
    }

    public void Create(Mark[] model)
    {
        throw new NotImplementedException();
    }

    public void DeleteById(Guid id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Mark> GetAll()
    {
        throw new NotImplementedException();
    }

    public Mark? GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public bool IdExists(Guid id)
    {
        throw new NotImplementedException();
    }
}
