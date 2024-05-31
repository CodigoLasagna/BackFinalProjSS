using Business.Contract;
using Domain;
using Helper.Connection;
using Helper.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;

namespace SSProyectoFinal.Controller;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost("registerWithKey/{rsa_id}")]
    public async Task<ActionResult> CreateRandomKeyUser(CreateUserModel entity, [FromRoute]int rsa_id)
    {
        User newUser = new User();
        newUser.Name = entity.Name;
        newUser.Email = entity.Email;
        newUser.Password = entity.Password;
        int userId = userService.Create(newUser, rsa_id);
        if (userId == -2)
            return BadRequest("Ese email ya esta registrado");
        if (userId == -3)
            return NotFound("Esa llave no existe");
        if (userId == -4)
            return NotFound("Aún no se han creado llaves rsa");
        if (userId <= 0)
            return BadRequest("error");
        return Ok(userId);
    }

    [HttpPost("Login")]
    public async Task<ActionResult> Login([FromBody]LoginUserModel entity)
    {
        string value = userService.Login(entity.Email, entity.Password);
        if (value == "-1") return BadRequest("No existe un usuario con ese correo");
        if (value == "-2") return BadRequest("Contraseña erronea");
        return Ok(value);
    }
    [HttpPut("Update/{Id}")]
    public async Task<ActionResult> Update([FromBody] UpdateUserModel entity, [FromRoute]int Id)
    {
        if (entity == null)
            return BadRequest("Json Vacio");
        User updatedUser = new User();
        updatedUser.Name = entity.Name;
        updatedUser.Email = entity.Email;
        updatedUser.Password = entity.Password;
        bool value = userService.Update(Id, updatedUser);
        return Ok(value);
    }
        [HttpPost("registerWithKeyNoProct/{rsa_id}")]
        public async Task<ActionResult> CreateRandomKeyUserNoProct(CreateUserModel entity, [FromRoute]int rsa_id)
        {
            var connectionOptions = DbConnection.GetDbContextOptions();
            using var ctx = new DBContextController(options: connectionOptions);

            RsaKey key;
            Random rng = new Random();
            if (rsa_id <= 0)
                rsa_id = rng.Next(0, ctx.RsaKeys.ToList().Count);
            key = ctx.RsaKeys.FirstOrDefault(x => x.Id == rsa_id);
            string malEm = "test '); DROP TABLE Users; (SELECT * FROM 'RsaKeys";
        
            // Construcción de la consulta SQL vulnerable
            string command = $"INSERT INTO Users (Name, Password, rsa_id, Email) " +
                             $"VALUES ('{entity.Name}', " +
                             $"'{RsaMath.Encrypt(entity.Password, key.Npart, key.Epart)}', " +
                             $"'{rsa_id}', " + // rsa_id como string (probablemente un error aquí)
                             $"'{entity.Email}')";
            ctx.Database.ExecuteSqlRaw(command);
            
            return Ok();
        }
    
        [HttpPost("LoginNoProct")]
        public async Task<ActionResult> LoginNoProct([FromBody]LoginUserModel entity)
        {
            var connectionOptions = DbConnection.GetDbContextOptions();
            using var ctx = new DBContextController(options: connectionOptions);
        
            // Simulación de inyección SQL al construir la consulta de selección
            string maliciousEmail = "' OR '1'='1"; // Inyectamos una condición siempre verdadera
            string query = $"SELECT * FROM Users WHERE Email = '{entity.Email}'"; // Aquí está la vulnerabilidad
            var users = ctx.Users.FromSqlRaw(query).ToList();
            Console.WriteLine(users.Count);
        
            var user = users.FirstOrDefault(); // Tomamos el primer usuario encontrado
        
            if (user == null)
                return BadRequest("Credenciales inválidas");
        
            RsaKey key = ctx.RsaKeys.FirstOrDefault(x => x.Id == user.rsa_id);
        
            // Validar la contraseña utilizando RSA (simulado)
            var decryptedPassword = RsaMath.Decrypt(user.Password, key.Npart, key.Dpart);
            if (decryptedPassword != entity.Password)
                return BadRequest("Contraseña inválida");
        
            // Login exitoso
            return Ok($"Login Exitoso {user.Name}");
        }
    
        [HttpPut("UpdateNoProct")]
        public async Task<ActionResult> UpdateNoProct([FromBody] UpdateUserModelNoS entity)
        {
            var connectionOptions = DbConnection.GetDbContextOptions();
            using var ctx = new DBContextController(options: connectionOptions);
    
            // Inyección SQL al construir la consulta de actualización
            string command = $"UPDATE Users SET Name = '{entity.Name}', Email = '{entity.Email}', Password = '{entity.Password}' WHERE Id = '{entity.Id}'";
            ctx.Database.ExecuteSqlRaw(command);
    
            return Ok(true);
        }
}