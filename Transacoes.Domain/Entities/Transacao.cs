using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Transacoes.Domain.Entities
{
    public class Transacao
    {
        [BsonId] // Atributo para indicar que esta propriedade � o ID prim�rio no MongoDB
        [BsonRepresentation(BsonType.ObjectId)] // Atributo para que o MongoDB gere um ObjectId para esta propriedade
        public string Id { get; set; }

        public decimal Valor { get; set; } // Valor da transa��o
        public string Moeda { get; set; } // Moeda da transa��o 
        public string Descricao { get; set; } // Descri��o da transa��o
        public DateTime Data { get; set; } // Data e hora da transa��o
    }
}