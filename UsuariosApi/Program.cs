using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UsuariosApi.Authorization;
using UsuariosApi.Data;
using UsuariosApi.Models;
using UsuariosApi.Services;

var builder = WebApplication.CreateBuilder(args);
//Variavel de conexão com banco
//Usando secrets para proteger a connectionString
var connectionString = builder.Configuration["ConnectionStrings:UsuarioConnection"];

// Add services to the container.
//Adicionando o contexto que vamos nos comunicar com o banco
builder.Services.AddDbContext<UsuarioDbContext>(
    options =>
    {
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    });

//Configurando Indetity
//Adicionando o conceito de identidade para classe Usuario e o papel de nosso usuario dentro do sistema sera gerenciado pelo Identity (IdentityRole)
//Estou utilizando o DbContext de usuario para fazer a comunicação com o banco. Quem ira armazenar as configurações do usuario é o DbContext
//Por fim, qual o token que iremos utilizar
builder.Services
    .AddIdentity<Usuario, IdentityRole>()
    .AddEntityFrameworkStores<UsuarioDbContext>()
    .AddDefaultTokenProviders();

//Adicionando AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Adicionando injeção das classes services de usuarios
//Podemos usar o AddScoped, AddTransient e AddSingleton. 
//AddScoped - CadastroService sempre vai ser instanciado quando houver uma requisição nova que demande uma instancia do CadastroService
//AddTransient - Sempre vai criar uma instancia nova de CadastroService, mesmo que seja na mesma requisição
//AddSingleton - Uma unica instancia de CadastroService para todas as requisições que chegassem
builder.Services.AddScoped<UsuarioService>();
//Adicionando Service de Token
builder.Services.AddScoped<TokenService>();

//Adicionando Authorizations
builder.Services.AddSingleton<IAuthorizationHandler, IdadeAuthorization>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Adicionando autenticação
builder.Services.AddAuthentication(options =>
{
    //Definindo autenticação padrão
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        //Validando a chave
        ValidateIssuerSigningKey = true,
        //Utilizando secrets para proteger a chave
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SymmetricSecurityKey"])),
        //Validando ataques de redirecionamento, não permitindo
        ValidateAudience = false,
        ValidateIssuer = false,
        ClockSkew = TimeSpan.Zero
    };
});

//Definido policy a ser usada e sua logica
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IdadeMinima", policy => policy.AddRequirements(new IdadeMinima(18)));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Dizendo que nossa aplicação usa autenticação para que funcione bem
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
