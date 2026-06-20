using MedFlow.Agendamento.Domain.Entities;
using MedFlow.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedFlow.Agendamento.Infrastructure.Repositories;

public class ConsultaRepository(AgendamentoDbContext context) 
    : IRepository<Consulta>
{
    public async Task<Consulta?> ObterPorIdAsync(Guid id, CancellationToken ct = default) =>
        await context.Consultas.FindAsync([id], ct);

    public async Task<IEnumerable<Consulta>> ObterTodosAsync(CancellationToken ct = default) =>
        await context.Consultas.ToListAsync(ct);

    public async Task AdicionarAsync(Consulta entity, CancellationToken ct = default) =>
        await context.Consultas.AddAsync(entity, ct);

    public Task AtualizarAsync(Consulta entity, CancellationToken ct = default)
    {
        context.Consultas.Update(entity);
        return Task.CompletedTask;
    }

    public async Task RemoverAsync(Guid id, CancellationToken ct = default)
    {
        var consulta = await ObterPorIdAsync(id, ct);
        if (consulta is not null)
            context.Consultas.Remove(consulta);
    }
}
