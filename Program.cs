using Microsoft.EntityFrameworkCore;
using ModelagemOrm.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ===== CONFIGURAÇÃO DO ENTITY FRAMEWORK COM POSTGRESQL =====
builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(
                builder.Configuration.GetConnectionString("PostgreSQLConnection"),
                npgsqlOptions => npgsqlOptions.CommandTimeout(30) // Timeout de comando
            )
            .EnableSensitiveDataLogging() // Log detalhado (apenas desenvolvimento)
            .EnableDetailedErrors()        // Erros detalhados (apenas desenvolvimento)
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();