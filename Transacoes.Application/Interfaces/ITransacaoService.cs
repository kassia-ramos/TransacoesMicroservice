using System.Collections.Generic;
using System.Threading.Tasks;
using Transacoes.Domain.Entities; // Precisamos referenciar a entidade Transacao

namespace Transacoes.Application.Interfaces
{
    public interface ITransacaoService
    {
        // M�todo para criar uma nova transa��o
        // Retorna a transa��o criada (com o ID gerado pelo banco de dados)
        Task<Transacao> CriarTransacaoAsync(Transacao transacao);

        // M�todo para obter todas as transa��es
        Task<IEnumerable<Transacao>> ObterTodasTransacoesAsync();
 
    }
}