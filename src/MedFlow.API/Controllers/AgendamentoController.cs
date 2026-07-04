using MedFlow.Agendamento.Application.Commands;
using MedFlow.Agendamento.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MedFlow.API.Controllers;

[Authorize]
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

    [HttpPut("consultas/{id:guid}/confirmar")]
    public async Task<IActionResult> ConfirmarConsulta(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new ConfirmarConsultaCommand(id), ct);
        if (!result) return NotFound("Consulta não encontrada.");
        return NoContent();
    }

    [HttpPut("consultas/{id:guid}/cancelar")]
    public async Task<IActionResult> CancelarConsulta(
        Guid id,
        [FromBody] CancelarConsultaRequest request,
        CancellationToken ct)
    {
        var result = await mediator.Send(new CancelarConsultaCommand(id, request.Motivo), ct);
        if (!result) return NotFound("Consulta não encontrada.");
        return NoContent();
    }
}

public record CancelarConsultaRequest(string Motivo);
