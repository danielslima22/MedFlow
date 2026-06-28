using MediatR;

namespace MedFlow.Agendamento.Application.Commands;

public record CancelarConsultaCommand(Guid ConsultaId, string Motivo) : IRequest<bool>;
