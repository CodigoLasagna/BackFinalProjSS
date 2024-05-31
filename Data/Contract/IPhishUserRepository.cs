using Domain;

namespace Data.Contract;

public interface IPhishUserRepository : IGenericRepository<PhishUser>
{
    public List<PhishUser> GetAllCapturedUsers();
}