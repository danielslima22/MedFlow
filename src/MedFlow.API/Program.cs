using FluentValidation;
using MedFlow.Agendamento.Domain.Entities;
using MedFlow.Agendamento.Infrastructure;
using MedFlow.Agendamento.Infrastructure.Repositories;
using MedFlow.API.Middlewares;
using MedFlow.SharedKernel.Behaviors;
using MedFlow.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Iniciando MedFlow.API...");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration)
                     .ReadFrom.Services(services)
                     .Enrich.FromLogContext());

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // EF Core + PostgreSQL
    builder.Services.AddDbContext<AgendamentoDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));

    // Repositorios
    builder.Services.AddScoped<IRepository<Consulta>, ConsultaRepository>();

    // UnitOfWork
    builder.Services.AddScoped<IUnitOfWork, AgendamentoUnitOfWork>();

    // MediatR + ValidationBehavior
    builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(
            typeof(MedFlow.Agendamento.Application.Commands.AgendarConsultaCommand).Assembly);
        cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
    });

    // FluentValidation
    builder.Services.AddValidatorsFromAssembly(
        typeof(MedFlow.Agendamento.Application.Commands.AgendarConsultaCommand).Assembly);

    var app = builder.Build();

    app.UseMiddleware<ErrorHandlingMiddleware>();
    app.UseSerilogRequestLogging();

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "MedFlow.API encerrou inesperadamente.");
}
finally
{
    Log.CloseAndFlush();
}