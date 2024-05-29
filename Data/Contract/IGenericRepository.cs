namespace Data.Contract;

public interface IGenericRepository<T> where T : class
{
    int Create(T entity);
    T Read(int Id);
    bool Update(int Id, T entity);
    bool Delete(int Id);
}