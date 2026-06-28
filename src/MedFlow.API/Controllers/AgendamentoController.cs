using MedFlow.Agendamento.Application.Commands;
using MedFlow.Agendamento.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MedFlow.API.Controllers;

[ApiController]
[Route("api/agendamento")]
public class AgendamentoController(IMediator mediator) : ControllerBase
{
    [HttpPost("consultas")]
    public async Task<IActionResult> AgendarConsulta(
        [FromBody] AgendarConsultaCommand command,
        CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(BuscarConsultas), new { id = result.ConsultaId }, result);
    }

    [HttpGet("consultas")]
    public async Task<IActionResult> BuscarConsultas(
        [FromQuery] Guid? medicoId,
        [FromQuery] Guid? pacienteId,
        [FromQuery] DateTime? dataInicio,
        [FromQuery] DateTime? dataFim,
        CancellationToken ct)
    {
        var query = new BuscarConsultasQuery(medicoId, pacienteId, dataInicio, dataFim);
        var result = await mediator.Send(query, ct);
        return Ok(result);
    }
}
