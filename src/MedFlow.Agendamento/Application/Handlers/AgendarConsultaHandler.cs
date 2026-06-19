using MediatR;
using MedFlow.Agendamento.Application.Commands;
using MedFlow.Agendamento.Domain.Entities;
using MedFlow.Agendamento.Domain.ValueObjects;
using MedFlow.SharedKernel.Interfaces;

namespace MedFlow.Agendamento.Application.Handlers;

public class AgendarConsultaHandler(
    IRepository<Consulta> repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<AgendarConsultaCommand, AgendarConsultaResult>
{
    public async Task<AgendarConsultaResult> Handle(
        AgendarConsultaCommand request,
        CancellationToken cancellationToken)
    {
        var dataHora = new DataHoraConsulta(request.DataHoraInicio, request.DuracaoMinutos);

        var consulta = Consulta.Agendar(
            request.PacienteId,
            request.MedicoId,
            dataHora,
            request.Observacao);

        await repository.AdicionarAsync(consulta, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return new AgendarConsultaResult(
            consulta.Id,
            dataHora.Inicio,
            dataHora.Fim,
            consulta.Status.ToString());
    }
}
