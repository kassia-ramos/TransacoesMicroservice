using System.Collections.Generic;
using System.Threading.Tasks;
using Transacoes.Domain.Entities; // Precisamos referenciar a entidade Transacao

namespace Transacoes.Application.Interfaces
{
    public interface ITransacaoService
    {
        // Método para criar uma nova transação
        // Retorna a transação criada (com o ID gerado pelo banco de dados)
        Task<Transacao> CriarTransacaoAsync(Transacao transacao);

        // Método para obter todas as transações
        Task<IEnumerable<Transacao>> ObterTodasTransacoesAsync();
 
    }
}