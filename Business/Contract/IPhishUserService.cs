using Data.Contract;
using Domain;

namespace Business.Contract;

public interface IPhishUserService : IGenericRepository<PhishUser>
{
    public List<PhishUser> GetAllCapturedUsers();
}