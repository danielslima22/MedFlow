namespace MedFlow.Agendamento.Domain.ValueObjects;

public record DataHoraConsulta
{
    public DateTime Inicio { get; init; }
    public DateTime Fim { get; init; }

    private DataHoraConsulta() { }

    public DataHoraConsulta(DateTime inicio, int duracaoMinutos = 30)
    {
        if (duracaoMinutos < 15 || duracaoMinutos > 120)
            throw new ArgumentException("Duracao deve ser entre 15 e 120 minutos.");

        Inicio = DateTime.SpecifyKind(inicio, DateTimeKind.Utc);
        
        if (Inicio <= DateTime.UtcNow)
            throw new ArgumentException("A consulta deve ser agendada para uma data futura.");

        Fim = Inicio.AddMinutes(duracaoMinutos);
    }

    public bool ConflitaCom(DataHoraConsulta outra) =>
        Inicio < outra.Fim && Fim > outra.Inicio;
}
