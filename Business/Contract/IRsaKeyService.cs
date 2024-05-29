using Domain;

namespace Business.Contract;

public interface IRsaKeyService : IGenericService<RsaKey>
{
    int GenerateRsaKey();
}