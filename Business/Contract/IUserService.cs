using Domain;

namespace Business.Contract;

public interface IUserService : IGenericService<User>
{
    int Create(User entity, int rsaId);
    string Login(string email, string password);
}