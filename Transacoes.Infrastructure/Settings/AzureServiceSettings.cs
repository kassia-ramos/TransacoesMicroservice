namespace Transacoes.Infrastructure.Settings
{
    public class AzureServiceBusSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string QueueName { get; set; } = string.Empty;
    }
}