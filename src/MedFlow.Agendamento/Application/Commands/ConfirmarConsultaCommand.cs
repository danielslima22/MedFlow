using MediatR;

namespace MedFlow.Agendamento.Application.Commands;

public record ConfirmarConsultaCommand(Guid ConsultaId) : IRequest<bool>;
