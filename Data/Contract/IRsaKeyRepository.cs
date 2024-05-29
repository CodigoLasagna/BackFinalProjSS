using Domain;

namespace Data.Contract;

public interface IRsaKeyRepository : IGenericRepository<RsaKey>
{
    int GenerateRsaKey();
}