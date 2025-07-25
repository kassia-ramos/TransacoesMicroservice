using System.Collections.Generic;
using System.Threading.Tasks;
using Transacoes.Application.Interfaces; // Referencia a interface ITransacaoService
using Transacoes.Domain.Entities;      // Referencia a entidade Transacao
using Transacoes.Domain.Interfaces;     // Referencia a interface ITransacaoRepository

namespace Transacoes.Application.Services // <-- Este � o namespace que faltava!
{
    public class TransacaoService : ITransacaoService
    {
        private readonly ITransacaoRepository _transacaoRepository;

        // Construtor: Recebe uma inst�ncia de ITransacaoRepository via inje��o de depend�ncia
        public TransacaoService(ITransacaoRepository transacaoRepository)
        {
            _transacaoRepository = transacaoRepository;
        }

        // Implementa��o do m�todo CriarTransacaoAsync
        public async Task<Transacao> CriarTransacaoAsync(Transacao transacao)
        {
            // Definir a data da transa��o se n�o for fornecida
            if (transacao.Data == default(DateTime))
            {
                transacao.Data = DateTime.UtcNow;
            }

            await _transacaoRepository.AdicionarAsync(transacao);
            return transacao; // Retorna a transa��o com o Id gerado pelo MongoDB
        }

        // Implementa��o do m�todo ObterTodasTransacoesAsync
        public async Task<IEnumerable<Transacao>> ObterTodasTransacoesAsync()
        {
            return await _transacaoRepository.ObterTodasAsync();
        }
    }
}