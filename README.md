# 🏥 MedFlow

Sistema de gestão para clínicas médicas — construído com .NET 10, Clean Architecture, DDD, CQRS e mensageria.

## Stack

- **Back-end:** ASP.NET Core 10, MediatR, EF Core 10
- **Banco relacional:** PostgreSQL
- **Banco de documentos:** MongoDB (prontuário)
- **Cache:** Redis
- **Mensageria:** RabbitMQ + MassTransit
- **Logs:** Serilog + Seq
- **Front-end:** Blazor WebAssembly + MudBlazor

## Subindo o ambiente

```bash
docker compose up -d
```

### Serviços disponíveis

| Serviço     | URL / Porta         |
|-------------|---------------------|
| PostgreSQL  | localhost:5432       |
| MongoDB     | localhost:27017      |
| Redis       | localhost:6379       |
| RabbitMQ    | http://localhost:15672 |
| Seq (logs)  | http://localhost:8081  |

### Credenciais (desenvolvimento)

- **Usuário:** medflow
- **Senha:** medflow@123

## Estrutura

```
src/
├── MedFlow.API              → Gateway principal
├── MedFlow.Agendamento      → Módulo de consultas
├── MedFlow.Prontuario       → Prontuário eletrônico
├── MedFlow.Notificacoes     → E-mail / SMS
├── MedFlow.Identity         → Autenticação JWT
├── MedFlow.SharedKernel     → Contratos e base DDD
└── MedFlow.Worker           → Background jobs
tests/
├── MedFlow.Agendamento.Tests
└── MedFlow.API.Tests
```
