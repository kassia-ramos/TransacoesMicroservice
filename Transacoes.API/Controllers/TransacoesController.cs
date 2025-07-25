using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transacoes.Application.Interfaces;
using Transacoes.Domain.Entities;
using Transacoes.API.Models; 

namespace Transacoes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransacoesController : ControllerBase
    {
        private readonly ITransacaoService _transacaoService;

        public TransacoesController(ITransacaoService transacaoService)
        {
            _transacaoService = transacaoService;
        }

        //  criar uma nova transação 
        [HttpPost]
        public async Task<ActionResult<Transacao>> CriarTransacao([FromBody] CriarTransacaoRequest request) // <<<< MUDOU AQUI
        {
            // Mapeia o DTO de requisição para a entidade de domínio
            var transacao = new Transacao
            {
                Valor = request.Valor,
                Moeda = request.Moeda,
                Descricao = request.Descricao,
                Data = request.Data
            };

            var transacaoCriada = await _transacaoService.CriarTransacaoAsync(transacao);

            // Retorna 201 Created com a transação criada e a URL para acessá-la
            return CreatedAtAction(nameof(ObterTodasTransacoes), new { id = transacaoCriada.Id }, transacaoCriada);
        }

        // listar todas as transações 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transacao>>> ObterTodasTransacoes()
        {
            var transacoes = await _transacaoService.ObterTodasTransacoesAsync();
            if (transacoes == null || !transacoes.Any())
            {
                return NotFound("Nenhuma transação encontrada.");
            }
            return Ok(transacoes);
        }
    }
}