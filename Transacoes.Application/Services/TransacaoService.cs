using System.Collections.Generic;
using System.Threading.Tasks;
using Transacoes.Application.Interfaces; // Referencia a interface ITransacaoService
using Transacoes.Domain.Entities;      // Referencia a entidade Transacao
using Transacoes.Domain.Interfaces;     // Referencia a interface ITransacaoRepository

namespace Transacoes.Application.Services // <-- Este é o namespace que faltava!
{
    public class TransacaoService : ITransacaoService
    {
        private readonly ITransacaoRepository _transacaoRepository;

        // Construtor: Recebe uma instância de ITransacaoRepository via injeção de dependência
        public TransacaoService(ITransacaoRepository transacaoRepository)
        {
            _transacaoRepository = transacaoRepository;
        }

        // Implementação do método CriarTransacaoAsync
        public async Task<Transacao> CriarTransacaoAsync(Transacao transacao)
        {
            // Definir a data da transação se não for fornecida
            if (transacao.Data == default(DateTime))
            {
                transacao.Data = DateTime.UtcNow;
            }

            await _transacaoRepository.AdicionarAsync(transacao);
            return transacao; // Retorna a transação com o Id gerado pelo MongoDB
        }

        // Implementação do método ObterTodasTransacoesAsync
        public async Task<IEnumerable<Transacao>> ObterTodasTransacoesAsync()
        {
            return await _transacaoRepository.ObterTodasAsync();
        }
    }
}