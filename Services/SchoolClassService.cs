using SchoolAPI.Data;
using SchoolAPI.Models;
using Microsoft.EntityFrameworkCore;

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
        #nullable disable
        return _context.Classes
            .Include(c => c.School)
            .Include(c => c.HeadMaster)
            .Include(c => c.HeadMaster.School)
            .AsNoTracking();
        #nullable enable
    }

    public Class? GetById(Guid id)
    {
        #nullable disable
        return _context.Classes
            .Include(c => c.School)
            .Include(c => c.HeadMaster)
            .Include(c => c.HeadMaster.School)
            .AsNoTracking()
            .FirstOrDefault(c => c.Id == id);
        #nullable enable
    }

    public bool IdExists(Guid id)
    {
        throw new NotImplementedException();
    }
}
