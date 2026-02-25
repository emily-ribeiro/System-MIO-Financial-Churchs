using SistemaIgreja.Components;
using SistemaIgreja.Data;
using SistemaIgreja.Services; 
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuração de Serviços
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// 2. Configura o Banco de Dados
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlite("Data Source=igreja.db"));


builder.Services.AddAuthentication("Cookies").AddCookie(options =>
{
   //Define a rota de login para redirecionar usuários não autenticados
    options.LoginPath = "/login"; 
});


builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
// Registra ProvedorSessao tanto como tipo concreto quanto como AuthenticationStateProvider
builder.Services.AddScoped<ProvedorSessao>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<ProvedorSessao>());
builder.Services.AddScoped<ProtectedLocalStorage>();


var app = builder.Build();

// 3. Configuração do Pipeline (Middleware)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();

// Adiciona os middlewares de autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();


app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// 4. Inicialização de Dados (Seed)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    // Aplica as migrações pendentes e cria o banco se não existir
    db.Database.Migrate();

    // Criar um usuário Admin inicial se o banco estiver vazio
    if (!db.Usuarios.Any())
    {
        db.Usuarios.Add(new Usuario { 
            Nome = "Admin Inicial", 
            Email = "admin@igreja.com", 
            Senha = "123", 
            Perfil = "Admin" 
        });
        db.SaveChanges();
    }
}

app.Run();