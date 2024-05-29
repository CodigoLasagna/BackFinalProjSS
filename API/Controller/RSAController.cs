using Business.Contract;
using Microsoft.AspNetCore.Mvc;

namespace SSProyectoFinal.Controller;

[ApiController]
[Route("api/[controller]")]
public class RSAController(IRsaKeyService rsaKeyService) : ControllerBase
{
    [HttpPost("GenerateKeys")]
    public async Task<ActionResult> GenerateKeys()
    {
        return Ok(rsaKeyService.GenerateRsaKey());
    }
}