using MedFlow.SharedKernel.Domain;

namespace MedFlow.SharedKernel.Interfaces;

public interface IRepository<T> where T : Entity
{
    Task<T?> ObterPorIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<T>> ObterTodosAsync(CancellationToken ct = default);
    Task AdicionarAsync(T entity, CancellationToken ct = default);
    Task AtualizarAsync(T entity, CancellationToken ct = default);
    Task RemoverAsync(Guid id, CancellationToken ct = default);
}
