using MediatR;
using MedFlow.Agendamento.Application.Commands;
using MedFlow.Agendamento.Domain.Entities;
using MedFlow.SharedKernel.Interfaces;

namespace MedFlow.Agendamento.Application.Handlers;

public class ConfirmarConsultaHandler(
    IRepository<Consulta> repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ConfirmarConsultaCommand, bool>
{
    public async Task<bool> Handle(
        ConfirmarConsultaCommand request,
        CancellationToken cancellationToken)
    {
        var consulta = await repository.ObterPorIdAsync(request.ConsultaId, cancellationToken);

        if (consulta is null)
            return false;

        consulta.Confirmar();

        await unitOfWork.CommitAsync(cancellationToken);
        return true;
    }
}
