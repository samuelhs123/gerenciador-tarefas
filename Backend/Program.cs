using GerenciadorTarefas.Data;
using GerenciadorTarefas.Middlewares;
using GerenciadorTarefas.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<GerenciadorTarefasDbContext>(options => 
        options.UseSqlServer(builder.Configuration.GetConnectionString("GerenciadorTarefasDb"), sqlServerOptions => sqlServerOptions.CommandTimeout(30))
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking), ServiceLifetime.Scoped);

builder.Services.AddTransient<GerenciadorTarefasService>();

builder.Services.AddCors();

var app = builder.Build();

app.ConfigureExceptionHandler();

app.UseCors(x => x
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
