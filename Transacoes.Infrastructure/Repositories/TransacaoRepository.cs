using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using Transacoes.Domain.Entities;      // Referencia a entidade Transacao
using Transacoes.Domain.Interfaces;     // Referencia a interface ITransacaoRepository
using Transacoes.Infrastructure.Settings; // Referencia as configura��es do Banco

namespace Transacoes.Infrastructure.Repositories
{
    public class TransacaoRepository : ITransacaoRepository
    {
        private readonly IMongoCollection<Transacao> _transacoesCollection;

        // Construtor: recebe as configura��es do banco
        public TransacaoRepository(IMongoClient mongoClient, MongoDbSettings settings)
        {
            // Usa o DatabaseName das configura��es para obter o banco de dados
            var database = mongoClient.GetDatabase(settings.DatabaseName);

            // Usa o CollectionName das configura��es para obter a cole��o
            _transacoesCollection = database.GetCollection<Transacao>(settings.CollectionName);
        }

        // para salvar uma transa��o no banco
        public async Task AdicionarAsync(Transacao transacao)
        {
            await _transacoesCollection.InsertOneAsync(transacao);
        }

        // para buscar todas as transa��es no banco
        public async Task<IEnumerable<Transacao>> ObterTodasAsync()
        {
            return await _transacoesCollection.Find(_ => true).ToListAsync();
        }
    }
}