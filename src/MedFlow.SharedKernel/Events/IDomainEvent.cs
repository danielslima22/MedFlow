using MediatR;

namespace MedFlow.SharedKernel.Events;

public interface IDomainEvent : INotification
{
    Guid EventId { get; }
    DateTime OcorridoEm { get; }
}
