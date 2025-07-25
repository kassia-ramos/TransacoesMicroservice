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

        //  criar uma nova transa��o 
        [HttpPost]
        public async Task<ActionResult<Transacao>> CriarTransacao([FromBody] CriarTransacaoRequest request) // <<<< MUDOU AQUI
        {
            // Mapeia o DTO de requisi��o para a entidade de dom�nio
            var transacao = new Transacao
            {
                Valor = request.Valor,
                Moeda = request.Moeda,
                Descricao = request.Descricao,
                Data = request.Data
            };

            var transacaoCriada = await _transacaoService.CriarTransacaoAsync(transacao);

            // Retorna 201 Created com a transa��o criada e a URL para acess�-la
            return CreatedAtAction(nameof(ObterTodasTransacoes), new { id = transacaoCriada.Id }, transacaoCriada);
        }

        // listar todas as transa��es 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transacao>>> ObterTodasTransacoes()
        {
            var transacoes = await _transacaoService.ObterTodasTransacoesAsync();
            if (transacoes == null || !transacoes.Any())
            {
                return NotFound("Nenhuma transa��o encontrada.");
            }
            return Ok(transacoes);
        }
    }
}