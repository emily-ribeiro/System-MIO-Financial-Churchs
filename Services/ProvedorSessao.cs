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

            // Atualiza a memória fotográfica com a congregação
            _usuarioEmMemoria = CriarPrincipal(usuario);

            return new AuthenticationState(_usuarioEmMemoria);
        }
        catch
        {
            return new AuthenticationState(_anonimo);
        }
    }

    public async Task Entrar(Usuario usuario)
    {
        _logger?.LogInformation("Entrar: storing session for {email}", usuario?.Email);
        try
        {
            await _storage.SetAsync("sessao_igreja", usuario);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Entrar: falha ao gravar sessao_igreja em ProtectedLocalStorage.");
            // Continuar: manter sessão em memória mesmo se storage falhar
        }

        // Atualiza a memória fotográfica com a congregação
        _usuarioEmMemoria = CriarPrincipal(usuario);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_usuarioEmMemoria)));
    }

    //ele transforma o objeto Usuario em um ClaimsPrincipal, que é o que o Blazor usa para controle de acesso.
    private ClaimsPrincipal CriarPrincipal(Usuario usuario)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Role, usuario.Perfil),
            new Claim("CongregacaoId", usuario.CongregacaoId?.ToString() ?? "0")
        };

        var identidade = new ClaimsIdentity(claims, "AutenticacaoSegura");
        return new ClaimsPrincipal(identidade);
    }

    public async Task Sair()
    {
        _logger?.LogInformation("Sair: clearing session and notifying anonymous state.");
        try
        {
            await _storage.DeleteAsync("sessao_igreja");
        }
        catch (Exception ex)
        {
            _logger?.LogWarning(ex, "Sair: erro ao deletar sessao_igreja do ProtectedLocalStorage.");
        }
        _usuarioEmMemoria = null;
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonimo)));
    }
}