using Domain;

namespace Data.Contract;

public interface IUserRepository : IGenericRepository<User>
{
    int Create(User entity, int rsaId);
    string Login(string email, string password);
}