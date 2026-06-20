using MedFlow.Agendamento.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MedFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AgendamentoController(IMediator mediator) : ControllerBase
{
    [HttpPost("consultas")]
    public async Task<IActionResult> AgendarConsulta(
        [FromBody] AgendarConsultaCommand command,
        CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(AgendarConsulta), new { id = result.ConsultaId }, result);
    }
}
