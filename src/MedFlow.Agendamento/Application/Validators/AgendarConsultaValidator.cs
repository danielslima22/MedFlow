using FluentValidation;
using MedFlow.Agendamento.Application.Commands;

namespace MedFlow.Agendamento.Application.Validators;

public class AgendarConsultaValidator : AbstractValidator<AgendarConsultaCommand>
{
    public AgendarConsultaValidator()
    {
        RuleFor(x => x.PacienteId)
            .NotEmpty().WithMessage("PacienteId e obrigatorio.");

        RuleFor(x => x.MedicoId)
            .NotEmpty().WithMessage("MedicoId e obrigatorio.");

        RuleFor(x => x.DataHoraInicio)
            .NotEmpty().WithMessage("Data e hora sao obrigatorias.")
            .GreaterThan(DateTime.UtcNow).WithMessage("A consulta deve ser agendada para uma data futura.");

        RuleFor(x => x.DuracaoMinutos)
            .InclusiveBetween(15, 120).WithMessage("Duracao deve ser entre 15 e 120 minutos.");
    }
}
