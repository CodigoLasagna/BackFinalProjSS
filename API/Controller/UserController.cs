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
            
            // Inyección SQL al construir la consulta de inserción
            string command = $"INSERT INTO Users (Name, Email, Password, rsa_id) VALUES ('{entity.Name}', '{entity.Email}', '{entity.Password}', {rsa_id})";
            ctx.Database.ExecuteSqlRaw(command);
            
            return Ok();
        }
    
        [HttpPost("LoginNoProct")]
        public async Task<ActionResult> LoginNoProct([FromBody]LoginUserModel entity)
        {
            var connectionOptions = DbConnection.GetDbContextOptions();
            using var ctx = new DBContextController(options: connectionOptions);
    
            // Inyección SQL al construir la consulta de selección
            string query = $"SELECT * FROM Users WHERE Email = '{entity.Email}'";
            var user = ctx.Users.FromSqlRaw(query).FirstOrDefault();
            if (user == null)
                return BadRequest("Credenciales invalidas");
            RsaKey key = ctx.RsaKeys.FirstOrDefault(x=>x.Id == user.rsa_id);
            if ((RsaMath.Decrypt(user.Password, key.Npart, key.Dpart) == entity.Password) == false)
                return BadRequest("contraseña invalida");
            if (user == null) return BadRequest("Credenciales invalidas");
            return Ok($"Login Exitoso {user.Name}");
        }
    
        [HttpPut("UpdateNoProct/{Id}")]
        public async Task<ActionResult> UpdateNoProct([FromBody] UpdateUserModel entity, [FromRoute]int Id)
        {
            var connectionOptions = DbConnection.GetDbContextOptions();
            using var ctx = new DBContextController(options: connectionOptions);
    
            // Inyección SQL al construir la consulta de actualización
            string command = $"UPDATE Users SET Name = '{entity.Name}', Email = '{entity.Email}', Password = '{entity.Password}' WHERE Id = {Id}";
            ctx.Database.ExecuteSqlRaw(command);
    
            return Ok(true);
        }
}