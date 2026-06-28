using FluentValidation;
using MedFlow.Agendamento.Domain.Entities;
using MedFlow.Agendamento.Infrastructure;
using MedFlow.Agendamento.Infrastructure.Repositories;
using MedFlow.SharedKernel.Behaviors;
using MedFlow.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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

app.UseMiddleware<MedFlow.API.Middlewares.ErrorHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();