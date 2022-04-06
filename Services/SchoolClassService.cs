using SchoolAPI.Data;
using SchoolAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace SchoolAPI.Services;

public class SchoolClassService : IService<SchoolClass>
{
    private readonly SchoolContext _context;

    public SchoolClassService(SchoolContext context)
    {
        _context = context;
    }

    public void Create(SchoolClass model)
    {
        throw new NotImplementedException();
    }

    public void Create(SchoolClass[] model)
    {
        throw new NotImplementedException();
    }

    public void DeleteById(Guid id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<SchoolClass> GetAll()
    {
        #nullable disable
        return _context.Classes
            .Include(c => c.School)
            .Include(c => c.HeadMaster)
            .Include(c => c.HeadMaster.School)
            .AsNoTracking();
        #nullable enable
    }

    public SchoolClass? GetById(Guid id)
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
