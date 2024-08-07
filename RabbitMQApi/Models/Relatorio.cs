namespace RabbitMQApi.Models;

public class Relatorio
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Status { get; set; }
    public DateTime? Data { get; set; }
}