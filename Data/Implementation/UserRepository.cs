using Data.Contract;
using Domain;
using Helper.Connection;
using Helper.Security;

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
    public int Create(User entity, int rsa_id)
    {
        var connectionOptions = DbConnection.GetDbContextOptions();
        using var ctx = new DBContextController(options: connectionOptions);
        if (rsa_id <= 0)
        {
            Random random = new Random();
            rsa_id = random.Next(1, ctx.RsaKeys.ToList().Count);
        }

        RsaKey key = ctx.RsaKeys.FirstOrDefault(x => x.Id == rsa_id);
        User existing_user = ctx.Users.FirstOrDefault(x => x.Email == entity.Email);
        if (existing_user != null)
            return -2;
            
        if (key == null && rsa_id > 0)
            return -3;
        if (key == null)
            return -4;

        entity.Password = RsaMath.Encrypt(entity.Password, key.Npart, key.Epart);

        entity.rsa_id = rsa_id;
        ctx.Users.Add(entity);
        ctx.SaveChanges();
        return entity.Id;
    }

    public string Login(string email, string password)
    {
        var connectionOptions = DbConnection.GetDbContextOptions();
        using var ctx = new DBContextController(options: connectionOptions);
        User existing_user = ctx.Users.FirstOrDefault(x => x.Email == email);
        if (existing_user == null) return "-1";
        RsaKey key = ctx.RsaKeys.FirstOrDefault(x => x.Id == existing_user.rsa_id);
        string decryptedPassword = RsaMath.Decrypt(existing_user.Password, key.Npart, key.Dpart);
        if (decryptedPassword == password)
        {
            string message;
            message = $"Nombre de usuario: {existing_user.Name}\n\nParte de N para llave privada y publica: {key.Npart}\n\nParte privada: {key.Dpart}\n\nParte publica: {key.Epart}";
            return message;
        }

        return "-2";
    }

    public User Read(int Id)
    {
        throw new NotImplementedException();
    }

    public bool Update(int Id, User entity)
    {
        var connectionOptions = DbConnection.GetDbContextOptions();
        using var ctx = new DBContextController(options: connectionOptions);
        User existing_user = ctx.Users.FirstOrDefault(x => x.Id == Id);
        if (existing_user == null) return false;
        RsaKey key = ctx.RsaKeys.FirstOrDefault(x => x.Id == existing_user.rsa_id);
        if (!string.IsNullOrEmpty(entity.Email) && entity.Email != existing_user.Email)
            existing_user.Email = entity.Email;
        if (!string.IsNullOrEmpty(entity.Name) && entity.Name != existing_user.Name)
            existing_user.Name = entity.Name;
        string processedPass = RsaMath.Decrypt(existing_user.Password, key.Npart, key.Dpart);
        if (!string.IsNullOrEmpty(entity.Password) && entity.Password != processedPass)
            existing_user.Password = RsaMath.Encrypt(entity.Password, key.Npart, key.Epart);
        ctx.Update(existing_user);
        ctx.SaveChanges();
        return true;
    }

    public bool Delete(int Id)
    {
        throw new NotImplementedException();
    }
}