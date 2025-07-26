using System.Collections.Generic;
using System.Threading.Tasks;
using Transacoes.Application.Interfaces; 
using Transacoes.Domain.Entities;
using Transacoes.Domain.Interfaces;

namespace Transacoes.Application.Services
{
    public class TransacaoService : ITransacaoService
    {
        private readonly ITransacaoRepository _transacaoRepository;
        private readonly IMensagemServiceBusProdutor _mensagemServiceProdutor;

        // Construtor: recebe ITransacaoRepository e IMensagemServiceProdutor
        public TransacaoService(
            ITransacaoRepository transacaoRepository,
            IMensagemServiceBusProdutor mensagemServiceProdutor)
        {
            _transacaoRepository = transacaoRepository;
            _mensagemServiceProdutor = mensagemServiceProdutor;
        }

        public async Task<Transacao> CriarTransacaoAsync(Transacao transacao)
        {
            if (transacao.Data == default(DateTime))
            {
                transacao.Data = DateTime.UtcNow;
            }

            await _transacaoRepository.AdicionarAsync(transacao);

            // Envia a transação para a fila do Service Bus APÓS salvar no MongoDB
            await _mensagemServiceProdutor.EnviarMensagemTransacaoAsync(transacao);

            return transacao;
        }

        public async Task<IEnumerable<Transacao>> ObterTodasTransacoesAsync()
        {
            return await _transacaoRepository.ObterTodasAsync();
        }

    }
}