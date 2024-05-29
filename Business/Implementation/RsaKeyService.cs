using Business.Contract;
using Data.Contract;
using Domain;

namespace Business.Implementation;

public class RsaKeyService(IRsaKeyRepository rsaKeyRepository) : IRsaKeyService
{
    public int Create(RsaKey entity)
    {
        throw new NotImplementedException();
    }

    public RsaKey Read(int Id)
    {
        throw new NotImplementedException();
    }

    public bool Update(int Id, RsaKey entity)
    {
        throw new NotImplementedException();
    }

    public bool Delete(int Id)
    {
        throw new NotImplementedException();
    }

    public int GenerateRsaKey()
    {
        return rsaKeyRepository.GenerateRsaKey();
    }
}