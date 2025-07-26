using System.Threading.Tasks;
using Transacoes.Domain.Entities; // Para poder passar a transacao

namespace Transacoes.Application.Interfaces
{
    public interface IMensagemServiceBusProdutor
    {
        Task EnviarMensagemTransacaoAsync(Transacao transacao);
    }
}