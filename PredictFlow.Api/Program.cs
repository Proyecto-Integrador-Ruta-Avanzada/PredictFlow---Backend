using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PredictFlow.Application.Interfaces;
using PredictFlow.Application.Interfaces.ExternalConnection;
using PredictFlow.Application.Interfaces.InvitationsInterfaces;
using PredictFlow.Application.Services;
using PredictFlow.Application.Settings;
using PredictFlow.Infrastructure.Persistence;
using PredictFlow.Infrastructure.Persistence.Repositories;
using PredictFlow.Domain.Interfaces;

var builder = WebApplication.CreateBuilder(args);

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

// 2. Registrar repositorios
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IBoardRepository, BoardRepository>();
builder.Services.AddScoped<IBoardColumnRepository, BoardColumnRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ISprintRepository, SprintRepository>();
builder.Services.AddScoped<ISprintTaskRepository, SprintTaskRepository>();
builder.Services.AddScoped<IInvitationsRepository, TeamInvitationRepository>();


// Registrar Servicios
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<ISprintTaskService, SprintTaskService>();
builder.Services.AddScoped<IInvitationLinkGenerator,InvitationLinkGenerator>();
builder.Services.AddHttpClient<IN8nWebhookService, N8nWebhookService>();
builder.Services.AddScoped<IInvitationService, InvitationsService>();
builder.Services.AddScoped<IBoardColumnService, BoardColumnService>();
builder.Services.AddScoped<IBoardService, BoardService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ISprintService, SprintService>();
builder.Services.AddScoped<IRiskService, RiskService>();
builder.Services.AddScoped<ITeamService, TeamService>();

// Configuración de Autenticación JWT (NUEVO BLOQUE)

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
{builder.Services.AddScoped<ISprintTaskService, SprintTaskService>();
builder.Services.AddScoped<IInvitationLinkGenerator,InvitationLinkGenerator>();
builder.Services.AddHttpClient<IN8nWebhookService, N8nWebhookService>();
builder.Services.AddScoped<IInvitationService, InvitationsService>();
builder.Services.AddScoped<IBoardColumnService, BoardColumnService>();
builder.Services.AddScoped<IBoardService, BoardService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ISprintService, SprintService>();
builder.Services.AddScoped<IRiskService, RiskService>();
builder.Services.AddScoped<ITeamService, TeamService>();
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

// 4. Registrar los controladores
builder.Services.AddControllers();

var app = builder.Build();

// 5. Configuración del pipeline
app.UseSwagger();
    
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "PredictFlow API v1");
    options.RoutePrefix = string.Empty; // Hace que Swagger sea la página inicial
});

app.UseHttpsRedirection();

// Middlewares de Seguridad (DEBEN IR AQUÍ)
app.UseAuthentication(); 
app.UseAuthorization();


// 6. Mapeo de los controladores
app.MapControllers();

app.Run();

//json {name": "Daniel Ariza","email": "danteariza85@gmail.com","password": "Qwe.123*"}//
