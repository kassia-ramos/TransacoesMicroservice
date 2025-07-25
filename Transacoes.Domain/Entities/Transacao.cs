using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Transacoes.Domain.Entities
{
    public class Transacao
    {
        [BsonId] // Atributo para indicar que esta propriedade é o ID primário no MongoDB
        [BsonRepresentation(BsonType.ObjectId)] // Atributo para que o MongoDB gere um ObjectId para esta propriedade
        public string Id { get; set; }

        public decimal Valor { get; set; } // Valor da transação
        public string Moeda { get; set; } // Moeda da transação 
        public string Descricao { get; set; } // Descrição da transação
        public DateTime Data { get; set; } // Data e hora da transação
    }
}