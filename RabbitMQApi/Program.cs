using MassTransit;
using RabbitMQApi.Consumers;
using RabbitMQApi.Events;
using RabbitMQApi.Hubs;
using RabbitMQApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<List<Relatorio>>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder
            .WithOrigins("https://localhost:7247")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.AddConsumer<RelatorioSolicitadoEventConsumer>();

    busConfigurator.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host("localhost", "/", hostConfigurator =>
        {
            hostConfigurator.Username("guest");
            hostConfigurator.Password("guest");
        });

        configurator.ConfigureEndpoints(context);
    });
});

builder.Services.AddSingleton<RelatorioSolicitadoEventConsumer>();
builder.Services.AddSingleton<IConsumer<RelatorioSolicitadoEvent>, RelatorioSolicitadoEventConsumer>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowSpecificOrigin");

app.MapControllers();
app.MapHub<RelatorioHub>("/relatorioHub");

app.Run();