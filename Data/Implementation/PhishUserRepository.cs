using Data.Contract;
using Domain;
using Helper.Connection;

namespace Data.Implementation;

public class PhishUserRepository : IPhishUserRepository
{
    public int Create(PhishUser entity)
    {
        var connectionOptions = DbConnection.GetDbContextOptions();
        using var ctx = new DBContextController(options: connectionOptions);
        ctx.PhishUsers.Add(entity);
        ctx.SaveChanges();
        return entity.Id;
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
        var connectionOptions = DbConnection.GetDbContextOptions();
        using var ctx = new DBContextController(options: connectionOptions);
        List<PhishUser> capturedUsers;
        capturedUsers = ctx.PhishUsers.ToList();
        return capturedUsers;
    }
}