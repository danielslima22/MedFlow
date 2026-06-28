using MediatR;
using MedFlow.Agendamento.Application.Commands;
using MedFlow.Agendamento.Domain.Entities;
using MedFlow.SharedKernel.Interfaces;

namespace MedFlow.Agendamento.Application.Handlers;

public class CancelarConsultaHandler(
    IRepository<Consulta> repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CancelarConsultaCommand, bool>
{
    public async Task<bool> Handle(
        CancelarConsultaCommand request,
        CancellationToken cancellationToken)
    {
        var consulta = await repository.ObterPorIdAsync(request.ConsultaId, cancellationToken);

        if (consulta is null)
            return false;

        consulta.Cancelar(request.Motivo);

        await unitOfWork.CommitAsync(cancellationToken);
        return true;
    }
}
