using MedFlow.SharedKernel.Domain;
using MedFlow.Agendamento.Domain.Events;
using MedFlow.Agendamento.Domain.ValueObjects;

namespace MedFlow.Agendamento.Domain.Entities;

public class Consulta : Entity
{
    public Guid PacienteId { get; private set; }
    public Guid MedicoId { get; private set; }
    public DataHoraConsulta DataHora { get; private set; } = null!;
    public StatusConsulta Status { get; private set; }
    public string? Observacao { get; private set; }

    private Consulta() { }

    public static Consulta Agendar(
        Guid pacienteId,
        Guid medicoId,
        DataHoraConsulta dataHora,
        string? observacao = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(pacienteId.ToString());
        ArgumentException.ThrowIfNullOrEmpty(medicoId.ToString());

        var consulta = new Consulta
        {
            PacienteId = pacienteId,
            MedicoId = medicoId,
            DataHora = dataHora,
            Status = StatusConsulta.Agendada,
            Observacao = observacao
        };

        consulta.AddDomainEvent(new ConsultaAgendadaEvent(
            consulta.Id,
            pacienteId,
            medicoId,
            dataHora.Inicio));

        return consulta;
    }

    public void Confirmar()
    {
        if (Status != StatusConsulta.Agendada)
            throw new InvalidOperationException("Apenas consultas agendadas podem ser confirmadas.");

        Status = StatusConsulta.Confirmada;
        AtualizadoEm = DateTime.UtcNow;
    }

    public void Cancelar(string motivo)
    {
        if (Status == StatusConsulta.Realizada)
            throw new InvalidOperationException("Consultas já realizadas não podem ser canceladas.");

        Status = StatusConsulta.Cancelada;
        Observacao = motivo;
        AtualizadoEm = DateTime.UtcNow;

        AddDomainEvent(new ConsultaCanceladaEvent(Id, PacienteId, MedicoId, motivo));
    }
}

public enum StatusConsulta
{
    Agendada,
    Confirmada,
    Realizada,
    Cancelada
}
