using MassTransit;
using Microsoft.AspNetCore.Mvc;
using RabbitMQApi.Events;
using RabbitMQApi.Models;

namespace RabbitMQApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RabbitMqController(IBus bus, List<Relatorio> relatorios) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] string nomeRelatorio)
    {
        var relatorio = new Relatorio
        {
            Id = Guid.NewGuid(),
            Nome = nomeRelatorio,
            Status = "Pendente",
            Data = null
        };

        relatorios.Add(relatorio);

        var eventRequest = new RelatorioSolicitadoEvent
        {
            Id = relatorio.Id,
            Nome = relatorio.Nome
        };

        await bus.Publish(eventRequest);

        return Ok(relatorio);
    }

    [HttpGet]
    public IActionResult Get()
    {
        if (!relatorios.Any())
        {
            return NotFound("Nenhum relatório encontrado.");
        }
        return Ok(relatorios);
    }
}