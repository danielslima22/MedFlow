using MedFlow.SharedKernel.Events;

namespace MedFlow.Agendamento.Domain.Events;

public record ConsultaAgendadaEvent(
    Guid ConsultaId,
    Guid PacienteId,
    Guid MedicoId,
    DateTime DataHora) : DomainEvent;
