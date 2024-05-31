using Business.Contract;
using Data.Contract;
using Domain;

namespace Business.Implementation;

public class PhishUserService(IPhishUserRepository phishUserRepository) : IPhishUserService
{
    public int Create(PhishUser entity)
    {
        return phishUserRepository.Create(entity);
    }

    public PhishUser Read(int Id)
    {
        throw new NotImplementedException();
    }

    public bool Update(int Id, PhishUser entity)
    {
        throw new NotImplementedException();
    }

    public bool Delete(int Id)
    {
        throw new NotImplementedException();
    }

    public List<PhishUser> GetAllCapturedUsers()
    {
        return phishUserRepository.GetAllCapturedUsers();
    }
}