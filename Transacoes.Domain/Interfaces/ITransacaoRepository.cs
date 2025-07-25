using System.Collections.Generic;
using System.Threading.Tasks;
using Transacoes.Domain.Entities; // Refer�ncia � classe Transacao

namespace Transacoes.Domain.Interfaces
{
    public interface ITransacaoRepository
    {
        // M�todo para adicionar uma nova transa��o ao reposit�rio
        Task AdicionarAsync(Transacao transacao);

        // M�todo para obter todas as transa��es do reposit�rio
        Task<IEnumerable<Transacao>> ObterTodasAsync();
    }
}