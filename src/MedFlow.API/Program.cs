using FluentValidation;
using MedFlow.Agendamento.Application.Commands;
using MedFlow.Agendamento.Domain.Entities;
using MedFlow.Agendamento.Infrastructure;
using MedFlow.Agendamento.Infrastructure.Repositories;
using MedFlow.API.Middlewares;
using MedFlow.Identity.Application.Commands;
using MedFlow.Identity.Application.Services;
using MedFlow.Identity.Infrastructure;
using MedFlow.SharedKernel.Behaviors;
using MedFlow.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;
using System.Text;



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

    // Scalar (substituto moderno do Swagger)
    builder.Services.AddOpenApi();

    // EF Core + PostgreSQL — Agendamento
    builder.Services.AddDbContext<AgendamentoDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));

    // EF Core + PostgreSQL — Identity
    builder.Services.AddDbContext<MedFlowIdentityDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));

    // ASP.NET Identity
    builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
    })
    .AddEntityFrameworkStores<MedFlowIdentityDbContext>()
    .AddDefaultTokenProviders();

    // JWT
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]!);

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Emissor"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audiencia"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

    builder.Services.AddAuthorization();

    // Repositorios
    builder.Services.AddScoped<IRepository<Consulta>, ConsultaRepository>();
    builder.Services.AddScoped<IUnitOfWork, AgendamentoUnitOfWork>();
    builder.Services.AddScoped<JwtTokenService>();

    // MediatR
    builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(
            typeof(AgendarConsultaCommand).Assembly);
        cfg.RegisterServicesFromAssembly(
            typeof(RegistrarUsuarioCommand).Assembly);
        cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
    });

    // FluentValidation
    builder.Services.AddValidatorsFromAssembly(
        typeof(AgendarConsultaCommand).Assembly);




    var app = builder.Build();

    app.UseMiddleware<ErrorHandlingMiddleware>();
    app.UseSerilogRequestLogging();

    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("MedFlow API")
               .WithTheme(Scalar.AspNetCore.ScalarTheme.DeepSpace)
               .WithDefaultHttpClient(Scalar.AspNetCore.ScalarTarget.CSharp, Scalar.AspNetCore.ScalarClient.HttpClient)
               .WithHttpBearerAuthentication(bearer =>
               {
                   bearer.Token = "seu-token-aqui";
               });
    });

    app.UseHttpsRedirection();
    app.UseAuthentication();
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