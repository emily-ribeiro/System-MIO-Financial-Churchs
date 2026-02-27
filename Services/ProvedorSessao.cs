using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using SistemaIgreja.Data;

namespace SistemaIgreja.Services;

public class ProvedorSessao : AuthenticationStateProvider
{
    private readonly ProtectedLocalStorage _storage;
    private readonly ClaimsPrincipal _anonimo = new(new ClaimsIdentity());
    private readonly ILogger<ProvedorSessao> _logger;
    private ClaimsPrincipal? _usuarioEmMemoria; 

    public ProvedorSessao(ProtectedLocalStorage storage, ILogger<ProvedorSessao> logger)
    {
        _storage = storage;
        _logger = logger;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            if (_usuarioEmMemoria != null)
                return new AuthenticationState(_usuarioEmMemoria);

            var resultado = await _storage.GetAsync<Usuario>("sessao_igreja");
            var usuario = resultado.Success ? resultado.Value : null;

            if (usuario == null)
            {
                _logger?.LogInformation("GetAuthenticationStateAsync: no session found in ProtectedLocalStorage.");
                return new AuthenticationState(_anonimo);
            }

            _usuarioEmMemoria = CriarPrincipal(usuario);
            return new AuthenticationState(_usuarioEmMemoria);
        }
        catch
        {
            return new AuthenticationState(_anonimo);
        }
    }

    // TRAVA 1: Aceita o usuário e já verifica se ele não é nulo antes de prosseguir
    public async Task Entrar(Usuario? usuario)
    {
        if (usuario == null) return; // Se vier nulo da tela de login, aborta em segurança

        _logger?.LogInformation("Entrar: storing session for {email}", usuario.Email);
        try
        {
            await _storage.SetAsync("sessao_igreja", usuario);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Entrar: falha ao gravar sessao_igreja.");
        }

        _usuarioEmMemoria = CriarPrincipal(usuario);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_usuarioEmMemoria)));
    }

    // TRAVA 2: Proteção total contra ArgumentNullException
    private ClaimsPrincipal CriarPrincipal(Usuario usuario)
    {
        var claims = new List<Claim>
        {
            // O operador ?? garante que, se o dado for nulo, ele não quebra a aplicação
            new Claim(ClaimTypes.Name, usuario.Nome ?? "Usuário Sem Nome"),
            new Claim(ClaimTypes.Email, usuario.Email ?? "sem-email@igreja.com"),
            new Claim(ClaimTypes.Role, usuario.Perfil ?? "Tesoureiro"),
            new Claim("CongregacaoId", usuario.CongregacaoId?.ToString() ?? "0")
        };

        var identidade = new ClaimsIdentity(claims, "AutenticacaoSegura");
        return new ClaimsPrincipal(identidade);
    }

    public async Task Sair()
    {
        _logger?.LogInformation("Sair: clearing session.");
        try
        {
            await _storage.DeleteAsync("sessao_igreja");
        }
        catch (Exception ex)
        {
            _logger?.LogWarning(ex, "Sair: erro ao deletar sessao_igreja.");
        }
        _usuarioEmMemoria = null;
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonimo)));
    }
}