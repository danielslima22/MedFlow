using MedFlow.SharedKernel.Interfaces;

namespace MedFlow.Agendamento.Infrastructure;

public class AgendamentoUnitOfWork(AgendamentoDbContext context) : IUnitOfWork
{
    public async Task<int> CommitAsync(CancellationToken ct = default) =>
        await context.SaveChangesAsync(ct);
}
