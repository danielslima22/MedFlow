using MedFlow.Identity.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MedFlow.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar(
        [FromBody] RegistrarUsuarioCommand command,
        CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        if (!result.Sucesso)
            return BadRequest(new { erros = result.Erros });
        return Ok(new { token = result.Token });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginCommand command,
        CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        if (!result.Sucesso)
            return Unauthorized(new { erros = result.Erros });
        return Ok(new { token = result.Token });
    }
}
