using MedFlow.SharedKernel.Events;

namespace MedFlow.Agendamento.Domain.Events;

public record ConsultaCanceladaEvent(
    Guid ConsultaId,
    Guid PacienteId,
    Guid MedicoId,
    string Motivo) : DomainEvent;
