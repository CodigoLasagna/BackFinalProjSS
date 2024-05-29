using Data.Contract;
using Domain;
using Helper.Connection;
using Helper.Security;

namespace Data.Implementation;

public class RsaKeyRepository : IRsaKeyRepository
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
        var connectionOptions = DbConnection.GetDbContextOptions();
        using var ctx = new DBContextController(options: connectionOptions);
        RsaKey newKey = new RsaKey();
        var (n, e, d) = RsaMath.GenerateRSAKeys(1024);
        newKey.Npart = n.ToString();
        newKey.Epart = e.ToString();
        newKey.Dpart = d.ToString();
        ctx.RsaKeys.Add(newKey);
        ctx.SaveChanges();
        return newKey.Id;
    }
}