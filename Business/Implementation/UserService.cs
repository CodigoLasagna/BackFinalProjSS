using Business.Contract;
using Data.Contract;
using Domain;
using Helper.Security;

namespace Business.Implementation;

public class UserService(IUserRepository userRepository) : IUserService
{
    public int Create(User entity)
    {
        RsaMath.GenerateRSAKeys(1024);
        return userRepository.Create(entity);
    }

    public User Read(int Id)
    {
        throw new NotImplementedException();
    }

    public bool Update(int Id, User entity)
    {
        throw new NotImplementedException();
    }

    public bool Delete(int Id)
    {
        throw new NotImplementedException();
    }
}