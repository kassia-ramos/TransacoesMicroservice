using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Threading.Tasks;
using Transacoes.Application.Interfaces; // Para IMensagemServiceProdutor
using Transacoes.Domain.Entities; // Para a entidade Transacao
using Transacoes.Infrastructure.Settings; // Para AzureServiceSettings

namespace Transacoes.Infrastructure.Services 
{
    public class ServiceBusProdutor : IMensagemServiceBusProdutor 
    {
        private readonly ServiceBusClient _serviceBusClient;
        private readonly string _queueName;

        public ServiceBusProdutor(ServiceBusClient serviceBusClient, IOptions<AzureServiceBusSettings> serviceBusSettings)
        {
            _serviceBusClient = serviceBusClient;
            _queueName = serviceBusSettings.Value.QueueName;
        }

        public async Task EnviarMensagemTransacaoAsync(Transacao transacao)
        {
            ServiceBusSender sender = _serviceBusClient.CreateSender(_queueName);
            var jsonTransacao = JsonSerializer.Serialize(transacao);
            ServiceBusMessage message = new ServiceBusMessage(jsonTransacao);

            try
            {
                await sender.SendMessageAsync(message);
                Console.WriteLine($"Transa��o {transacao.Id} enviada para a fila: {_queueName}");
            }
            finally
            {
                await sender.DisposeAsync();
            }
        }
    }
}