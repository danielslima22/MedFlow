namespace MedFlow.Agendamento.Domain.ValueObjects;

public record DataHoraConsulta
{
    public DateTime Inicio { get; }
    public DateTime Fim { get; }

    public DataHoraConsulta(DateTime inicio, int duracaoMinutos = 30)
    {
        if (inicio <= DateTime.UtcNow)
            throw new ArgumentException("A consulta deve ser agendada para uma data futura.");

        if (duracaoMinutos < 15 || duracaoMinutos > 120)
            throw new ArgumentException("Duração deve ser entre 15 e 120 minutos.");

        Inicio = inicio;
        Fim = inicio.AddMinutes(duracaoMinutos);
    }

    public bool ConflitaCom(DataHoraConsulta outra) =>
        Inicio < outra.Fim && Fim > outra.Inicio;
}
