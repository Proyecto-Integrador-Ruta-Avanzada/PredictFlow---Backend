using Microsoft.EntityFrameworkCore;
using PredictFlow.Infrastructure.Persistence;
using PredictFlow.Infrastructure.Persistence.Repositories;
using PredictFlow.Domain.Interfaces;
using PredictFlow.Application.Interfaces;
using PredictFlow.Application.Services;
using PredictFlow.Application.Settings; 
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;             
using System.Text;                                

var builder = WebApplication.CreateBuilder(args);

// 0. Cargar y Configurar JwtSettings
// Mapea la sección "JwtSettings" del appsettings.json a la clase C# JwtSettings
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

// 1. Registrar DbContext con MySQL (Aiven)
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

// 2. Registrar repositorios y servicios de aplicación
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IBoardRepository, BoardRepository>();
builder.Services.AddScoped<IBoardColumnRepository, BoardColumnRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ISprintRepository, SprintRepository>();

// Servicios de Autenticación
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<AuthService>();

// 2.5. Configuración de Autenticación JWT (NUEVO BLOQUE)

// Obtenemos las JwtSettings para configurar la validación del token
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

// Configura el esquema de Autenticación JWT Bearer
builder.Services.AddAuthentication(options =>
{
    // Define JWT Bearer como el esquema por defecto para autenticar y responder a desafíos
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Deshabilitar solo en desarrollo (por simplicidad)
    options.SaveToken = true;
    
    // Parámetros de Validación (CRUCIALES)
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        // La clave secreta debe ser leída en formato byte
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings!.SecretKey)),
        
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer,
        
        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience,
        
        // No permite tolerancia de tiempo de reloj (ClockSkew)
        ClockSkew = TimeSpan.Zero,
        
        // Valida que el token no haya expirado
        ValidateLifetime = true 
    };
});

// Agrega el servicio de Autorización.
builder.Services.AddAuthorization();

// 3. Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4. Configuración del pipeline (Middleware)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    // Hacer que Swagger abra directamente en "/"
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "PredictFlow API v1");
        options.RoutePrefix = string.Empty; //  hace que swagger sea la página inicial
    });
}

app.UseHttpsRedirection();

// 4.5. Middlewares de Seguridad (DEBEN IR AQUÍ)
app.UseAuthentication(); 
app.UseAuthorization(); 

// (A futuro: aquí irán tus endpoints reales)
app.Run();