namespace SchoolAPI.Services;

public interface IService<T>
{
    public IEnumerable<T> GetAll();

    public T? GetById(Guid id);

    public void Create(T model);

    public void Create(T[] model);

    public bool IdExists(Guid id);
}