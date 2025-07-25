using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using Transacoes.Domain.Entities;      // Referencia a entidade Transacao
using Transacoes.Domain.Interfaces;     // Referencia a interface ITransacaoRepository
using Transacoes.Infrastructure.Settings; // Referencia as configurações do Banco

namespace Transacoes.Infrastructure.Repositories
{
    public class TransacaoRepository : ITransacaoRepository
    {
        private readonly IMongoCollection<Transacao> _transacoesCollection;

        // Construtor: recebe as configurações do banco
        public TransacaoRepository(IMongoClient mongoClient, MongoDbSettings settings)
        {
            // Usa o DatabaseName das configurações para obter o banco de dados
            var database = mongoClient.GetDatabase(settings.DatabaseName);

            // Usa o CollectionName das configurações para obter a coleção
            _transacoesCollection = database.GetCollection<Transacao>(settings.CollectionName);
        }

        // para salvar uma transação no banco
        public async Task AdicionarAsync(Transacao transacao)
        {
            await _transacoesCollection.InsertOneAsync(transacao);
        }

        // para buscar todas as transações no banco
        public async Task<IEnumerable<Transacao>> ObterTodasAsync()
        {
            return await _transacoesCollection.Find(_ => true).ToListAsync();
        }
    }
}