using MediatR;

namespace MedFlow.Agendamento.Application.Queries;

public record BuscarConsultasQuery(
    Guid? MedicoId = null,
    Guid? PacienteId = null,
    DateTime? DataInicio = null,
    DateTime? DataFim = null) : IRequest<IEnumerable<ConsultaDto>>;

public record ConsultaDto(
    Guid Id,
    Guid PacienteId,
    Guid MedicoId,
    DateTime DataHoraInicio,
    DateTime DataHoraFim,
    string Status,
    string? Observacao);
