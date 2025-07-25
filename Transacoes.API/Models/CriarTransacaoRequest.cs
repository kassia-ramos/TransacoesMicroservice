using System.ComponentModel.DataAnnotations; 
using System;

namespace Transacoes.API.Models 
{
    public class CriarTransacaoRequest
    {
        [Required(ErrorMessage = "O valor da transação é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "A moeda da transação é obrigatória.")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "A moeda deve ter 3 caracteres (ex: BRL, USD).")]
        public string Moeda { get; set; }

        [Required(ErrorMessage = "A descrição da transação é obrigatória.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "A data da transação é obrigatória.")]
        public DateTime Data { get; set; }
    }
}