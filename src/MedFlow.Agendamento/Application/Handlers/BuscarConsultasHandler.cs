using MediatR;
using MedFlow.Agendamento.Application.Queries;
using MedFlow.Agendamento.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MedFlow.Agendamento.Application.Handlers;

public class BuscarConsultasHandler(AgendamentoDbContext context)
    : IRequestHandler<BuscarConsultasQuery, IEnumerable<ConsultaDto>>
{
    public async Task<IEnumerable<ConsultaDto>> Handle(
        BuscarConsultasQuery request,
        CancellationToken cancellationToken)
    {
        var query = context.Consultas.AsNoTracking();

        if (request.MedicoId.HasValue)
            query = query.Where(c => c.MedicoId == request.MedicoId.Value);

        if (request.PacienteId.HasValue)
            query = query.Where(c => c.PacienteId == request.PacienteId.Value);

        if (request.DataInicio.HasValue)
            query = query.Where(c => c.DataHora.Inicio >= request.DataInicio.Value);

        if (request.DataFim.HasValue)
            query = query.Where(c => c.DataHora.Inicio <= request.DataFim.Value);

        return await query
            .OrderBy(c => c.DataHora.Inicio)
            .Select(c => new ConsultaDto(
                c.Id,
                c.PacienteId,
                c.MedicoId,
                c.DataHora.Inicio,
                c.DataHora.Fim,
                c.Status.ToString(),
                c.Observacao))
            .ToListAsync(cancellationToken);
    }
}
