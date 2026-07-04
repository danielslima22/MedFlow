using MediatR;

namespace MedFlow.Identity.Application.Commands;

public record RegistrarUsuarioCommand(
    string Email,
    string Senha,
    string ConfirmacaoSenha) : IRequest<AutenticacaoResult>;

public record LoginCommand(
    string Email,
    string Senha) : IRequest<AutenticacaoResult>;

public record AutenticacaoResult(
    bool Sucesso,
    string? Token = null,
    IEnumerable<string>? Erros = null);
