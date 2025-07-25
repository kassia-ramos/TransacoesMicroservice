using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options; 

using Transacoes.Domain.Entities;
using Transacoes.Domain.Interfaces;
using Transacoes.Infrastructure.Settings;

namespace Transacoes.Infrastructure.Repositories
{
    public class TransacaoRepository : ITransacaoRepository
    {
        private readonly IMongoCollection<Transacao> _transacoesCollection;

        // Construtor modificado: Agora recebe IOptions<MongoDbSettings>
        public TransacaoRepository(IMongoClient mongoClient, IOptions<MongoDbSettings> settings)
        {
            //Acesso
            var mongoDbSettings = settings.Value;

            // Usa o DatabaseName das configurações para obter o banco de dados
            var database = mongoClient.GetDatabase(mongoDbSettings.DatabaseName);

            // Usa o CollectionName das configurações para obter a coleção
            _transacoesCollection = database.GetCollection<Transacao>(mongoDbSettings.CollectionName);
        }

        public async Task AdicionarAsync(Transacao transacao)
        {
            await _transacoesCollection.InsertOneAsync(transacao);
        }

        public async Task<IEnumerable<Transacao>> ObterTodasAsync()
        {
            return await _transacoesCollection.Find(_ => true).ToListAsync();
        }
    }
}