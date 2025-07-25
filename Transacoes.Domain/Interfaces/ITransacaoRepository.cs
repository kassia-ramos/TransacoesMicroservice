using System.Collections.Generic;
using System.Threading.Tasks;
using Transacoes.Domain.Entities; // Referência à classe Transacao

namespace Transacoes.Domain.Interfaces
{
    public interface ITransacaoRepository
    {
        // Método para adicionar uma nova transação ao repositório
        Task AdicionarAsync(Transacao transacao);

        // Método para obter todas as transações do repositório
        Task<IEnumerable<Transacao>> ObterTodasAsync();
    }
}