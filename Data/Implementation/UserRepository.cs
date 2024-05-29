using Data.Contract;
using Domain;
using Helper.Connection;

namespace Data.Implementation;

public class UserRepository : IUserRepository
{
    public int Create(User entity)
    {
        var connectionOptions = DbConnection.GetDbContextOptions();
        using var ctx = new DBContextController(options: connectionOptions);
        ctx.Users.Add(entity);
        ctx.SaveChanges();
        return entity.Id;
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