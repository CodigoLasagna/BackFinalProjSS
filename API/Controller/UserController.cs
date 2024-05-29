using Business.Contract;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace SSProyectoFinal.Controller;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost("create")]
    [HttpPost("register")]
    public async Task<ActionResult> Create(CreateUserModel entity)
    {
        User newUser = new User();
        newUser.Name = entity.Name;
        newUser.Email = entity.Email;
        newUser.Password = entity.Password;
        int userId = userService.Create(newUser);
        if (userId <= 0)
            return BadRequest("error");
        return Ok(userId);
    }
}