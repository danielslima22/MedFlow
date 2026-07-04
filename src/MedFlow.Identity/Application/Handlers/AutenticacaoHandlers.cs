using MediatR;
using MedFlow.Identity.Application.Commands;
using MedFlow.Identity.Application.Services;
using Microsoft.AspNetCore.Identity;

namespace MedFlow.Identity.Application.Handlers;

public class RegistrarUsuarioHandler(
    UserManager<IdentityUser> userManager,
    JwtTokenService tokenService)
    : IRequestHandler<RegistrarUsuarioCommand, AutenticacaoResult>
{
    public async Task<AutenticacaoResult> Handle(
        RegistrarUsuarioCommand request,
        CancellationToken cancellationToken)
    {
        if (request.Senha != request.ConfirmacaoSenha)
            return new AutenticacaoResult(false, Erros: ["As senhas nao conferem."]);

        var user = new IdentityUser { UserName = request.Email, Email = request.Email };
        var result = await userManager.CreateAsync(user, request.Senha);

        if (!result.Succeeded)
            return new AutenticacaoResult(false, Erros: result.Errors.Select(e => e.Description));

        var token = tokenService.GerarToken(user);
        return new AutenticacaoResult(true, Token: token);
    }
}

public class LoginHandler(
    UserManager<IdentityUser> userManager,
    SignInManager<IdentityUser> signInManager,
    JwtTokenService tokenService)
    : IRequestHandler<LoginCommand, AutenticacaoResult>
{
    public async Task<AutenticacaoResult> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return new AutenticacaoResult(false, Erros: ["Email ou senha invalidos."]);

        var result = await signInManager.CheckPasswordSignInAsync(user, request.Senha, false);
        if (!result.Succeeded)
            return new AutenticacaoResult(false, Erros: ["Email ou senha invalidos."]);

        var token = tokenService.GerarToken(user);
        return new AutenticacaoResult(true, Token: token);
    }
}
