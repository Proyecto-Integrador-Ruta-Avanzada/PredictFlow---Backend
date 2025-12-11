using Microsoft.EntityFrameworkCore;
using PredictFlow.Infrastructure.Persistence;
using PredictFlow.Infrastructure.Persistence.Repositories;
using PredictFlow.Domain.Interfaces;

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
// 3. Swagger / OpenAPI
// ---------------------------------------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ---------------------------------------------------------
// 4. Configuración del pipeline
// ---------------------------------------------------------
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

// (A futuro: aquí irán tus endpoints reales)

// ---------------------------------------------------------
app.Run();
