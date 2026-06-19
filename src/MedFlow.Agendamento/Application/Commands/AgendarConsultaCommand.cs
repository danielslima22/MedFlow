using MediatR;

namespace MedFlow.Agendamento.Application.Commands;

public record AgendarConsultaCommand(
    Guid PacienteId,
    Guid MedicoId,
    DateTime DataHoraInicio,
    int DuracaoMinutos = 30,
    string? Observacao = null) : IRequest<AgendarConsultaResult>;

public record AgendarConsultaResult(
    Guid ConsultaId,
    DateTime DataHoraInicio,
    DateTime DataHoraFim,
    string Status);
