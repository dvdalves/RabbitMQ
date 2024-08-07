using MassTransit;
using RabbitMQApi.Events;
using RabbitMQApi.Models;

namespace RabbitMQApi.Consumers;

public class RelatorioSolicitadoEventConsumer(ILogger<RelatorioSolicitadoEventConsumer> logger, List<Relatorio> relatorios) : IConsumer<RelatorioSolicitadoEvent>
{
    public async Task Consume(ConsumeContext<RelatorioSolicitadoEvent> context)
    {
        var message = context.Message;

        logger.LogInformation($"Processando relatório: {context.Message.Nome}", message.Id, message.Nome);

        await Task.Delay(5000);

        var relatorio = relatorios.FirstOrDefault(x => x.Id == message.Id);

        if (relatorio != null)
        {
            relatorio.Status = "Concluído";
            relatorio.Data = DateTime.Now;
        }

        logger.LogInformation($"Relatório processado: {context.Message.Nome}", message.Id, message.Nome);
    }
}