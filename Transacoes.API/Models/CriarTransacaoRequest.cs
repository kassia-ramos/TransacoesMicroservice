using System.ComponentModel.DataAnnotations; 
using System;

namespace Transacoes.API.Models 
{
    public class CriarTransacaoRequest
    {
        [Required(ErrorMessage = "O valor da transa��o � obrigat�rio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "A moeda da transa��o � obrigat�ria.")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "A moeda deve ter 3 caracteres (ex: BRL, USD).")]
        public string Moeda { get; set; }

        [Required(ErrorMessage = "A descri��o da transa��o � obrigat�ria.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "A data da transa��o � obrigat�ria.")]
        public DateTime Data { get; set; }
    }
}