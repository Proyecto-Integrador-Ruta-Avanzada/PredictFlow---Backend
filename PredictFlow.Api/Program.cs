using Microsoft.EntityFrameworkCore;
using PredictFlow.Infrastructure.Persistence;
using PredictFlow.Infrastructure.Persistence.Repositories;
using PredictFlow.Domain.Interfaces;
// Nuevos usings necesarios para Auth
using PredictFlow.Application.Interfaces;
using PredictFlow.Application.Services;
using PredictFlow.Application.Settings;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------
// 1. Registrar DbContext con MySQL (Aiven)
// ---------------------------------------------------------
builder.Services.AddDbContext<PredictFlowDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString),
        mySqlOptions =>
        {
            mySqlOptions.EnableRetryOnFailure();
        });
});

// ---------------------------------------------------------
// 2. Registrar repositorios
// ---------------------------------------------------------
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IBoardRepository, BoardRepository>();
builder.Services.AddScoped<IBoardColumnRepository, BoardColumnRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ISprintRepository, SprintRepository>();

// ---------------------------------------------------------
// 3. Registrar Servicios de Aplicación (Auth y Token)
// ---------------------------------------------------------
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// ---------------------------------------------------------
// 4. Configuración de JWT (Autenticación)
// ---------------------------------------------------------
// Mapeamos la sección del appsettings a la clase JwtSettings
var jwtSection = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSection);

// Obtenemos los valores para configurar el validador
var jwtSettings = jwtSection.Get<JwtSettings>();
var key = Encoding.ASCII.GetBytes(jwtSettings!.SecretKey);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false; // En producción debería ser true
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// ---------------------------------------------------------
// 5. Controladores y Swagger
// ---------------------------------------------------------
builder.Services.AddControllers(); // Importante: Habilita los controladores
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ---------------------------------------------------------
// 6. Configuración del pipeline
// ---------------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    // Hacer que Swagger abra directamente en "/"
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "PredictFlow API v1");
        options.RoutePrefix = string.Empty; // hace que swagger sea la página inicial
    });
}

app.UseHttpsRedirection();

// ---------------------------------------------------------
// 7. Middlewares de Autenticación y Autorización
// ---------------------------------------------------------
// El orden es vital: Primero Auth (¿Quién eres?), luego Authorization (¿Qué puedes hacer?)
app.UseAuthentication();
app.UseAuthorization();

// Mapear los controladores para que los endpoints funcionen
app.MapControllers();

app.Run();