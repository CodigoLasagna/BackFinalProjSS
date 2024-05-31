using Business.Contract;
using Data.Contract;
using Domain;
using Helper.Security;

namespace Business.Implementation;

public class UserService(IUserRepository userRepository) : IUserService
{
    public int Create(User entity)
    {
        return userRepository.Create(entity);
    }

    public int Create(User entity, int rsa_key)
    {
        return userRepository.Create(entity, rsa_key);
    }

    public string Login(string email, string password)
    {
        return userRepository.Login(email, password);
    }


    public User Read(int Id)
    {
        throw new NotImplementedException();
    }

    public bool Update(int Id, User entity)
    {
        return userRepository.Update(Id, entity);
    }

    public bool Delete(int Id)
    {
        throw new NotImplementedException();
    }
}