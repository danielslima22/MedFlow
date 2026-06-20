using MedFlow.Agendamento.Infrastructure;
using MedFlow.Agendamento.Infrastructure.Repositories;
using MedFlow.Agendamento.Domain.Entities;
using MedFlow.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// EF Core + PostgreSQL
builder.Services.AddDbContext<AgendamentoDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));

// Repositórios
builder.Services.AddScoped<IRepository<Consulta>, ConsultaRepository>();

// UnitOfWork
builder.Services.AddScoped<IUnitOfWork, AgendamentoUnitOfWork>();

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(
        typeof(MedFlow.Agendamento.Application.Commands.AgendarConsultaCommand).Assembly));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();