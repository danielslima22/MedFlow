using MedFlow.Agendamento.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedFlow.Agendamento.Infrastructure;

public class AgendamentoDbContext(DbContextOptions<AgendamentoDbContext> options) 
    : DbContext(options)
{
    public DbSet<Consulta> Consultas => Set<Consulta>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Consulta>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.PacienteId).IsRequired();
            entity.Property(c => c.MedicoId).IsRequired();
            entity.Property(c => c.Status).HasConversion<string>();
            entity.OwnsOne(c => c.DataHora, dh =>
            {
                dh.Property(d => d.Inicio).HasColumnName("DataHoraInicio").IsRequired();
                dh.Property(d => d.Fim).HasColumnName("DataHoraFim").IsRequired();
            });
        });
    }
}
