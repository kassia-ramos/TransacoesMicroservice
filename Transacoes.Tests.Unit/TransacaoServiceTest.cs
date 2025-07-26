using Moq; 
using Xunit; 
using System;
using System.Threading.Tasks;
using System.Collections.Generic; 
using System.Linq; 

using Transacoes.Application.Interfaces; 
using Transacoes.Application.Services; 
using Transacoes.Domain.Entities; 
using Transacoes.Domain.Interfaces; 

namespace Transacoes.Tests.Unit
{
    public class TransacaoServiceTests
    {
        // Teste para o método CriarTransacaoAsync
        [Fact]
        public async Task CriarTransacaoAsync_DeveAdicionarTransacaoAoRepositorioEEnviarParaFila()
        {
            // Criar um mock para ITransacaoRepository
            var mockTransacaoRepository = new Mock<ITransacaoRepository>();
            // quando AdicionarAsync for chamado, apenas complete a Task.
            mockTransacaoRepository.Setup(repo => repo.AdicionarAsync(It.IsAny<Transacao>()))
                                 .Returns(Task.CompletedTask);

            // Criar um mock para IMensagemServiceBusProdutor
            var mockMensagemServiceProdutor = new Mock<IMensagemServiceBusProdutor>();
            // quando EnviarMensagemTransacaoAsync for chamado, apenas complete a Task.
            mockMensagemServiceProdutor.Setup(prod => prod.EnviarMensagemTransacaoAsync(It.IsAny<Transacao>()))
                                     .Returns(Task.CompletedTask);

            // Instanciar o serviço para testar, injetando os mocks como dependências
            var service = new TransacaoService(
                mockTransacaoRepository.Object,
                mockMensagemServiceProdutor.Object
            );

            // Criar uma transação de exemplo para o teste
            var transacaoDeTeste = new Transacao
            {
                Valor = 100.00m,
                Moeda = "BRL",
                Descricao = "Teste de criação de transação",
                Data = DateTime.UtcNow
            };

            // Executar a ação de teste
            var transacaoCriada = await service.CriarTransacaoAsync(transacaoDeTeste);

            // Verificar os resultados
            // Verificar se o método AdicionarAsync do repositório foi chamado exatamente uma vez com a transação correta
            mockTransacaoRepository.Verify(repo => repo.AdicionarAsync(transacaoDeTeste), Times.Once);

            // Verificar se o método EnviarMensagemTransacaoAsync do produtor de mensagens foi chamado exatamente uma vez com a transação correta
            mockMensagemServiceProdutor.Verify(prod => prod.EnviarMensagemTransacaoAsync(transacaoDeTeste), Times.Once);

            // Verificar se a transação retornada não é nula
            Assert.NotNull(transacaoCriada);
            
        }

        // Teste para ObterTodasTransacoesAsync
        [Fact]
        public async Task ObterTodasTransacoesAsync_DeveRetornarTodasAsTransacoesDoRepositorio()
        {
        
            var mockTransacaoRepository = new Mock<ITransacaoRepository>();
            var mockMensagemServiceProdutor = new Mock<IMensagemServiceBusProdutor>();

            // Definir uma lista de transações que o mock do repositório deve retornar
            var transacoesEsperadas = new List<Transacao>
            {
                new Transacao { Id = "id1", Valor = 10m, Moeda = "USD", Descricao = "Teste GET 1", Data = DateTime.UtcNow },
                new Transacao { Id = "id2", Valor = 20m, Moeda = "EUR", Descricao = "Teste GET 2", Data = DateTime.UtcNow.AddHours(-1) }
            };

            // Configurar o mock do repositório para retornar as transacoesEsperadas quando ObterTodasAsync for chamado
            mockTransacaoRepository.Setup(repo => repo.ObterTodasAsync())
                                 .ReturnsAsync(transacoesEsperadas);

            // Instanciar o serviço 
            var service = new TransacaoService(
                mockTransacaoRepository.Object,
                mockMensagemServiceProdutor.Object
            );

            
            var transacoesRetornadas = await service.ObterTodasTransacoesAsync();

            // Verificar se o método ObterTodasAsync do repositório foi chamado exatamente uma vez
            mockTransacaoRepository.Verify(repo => repo.ObterTodasAsync(), Times.Once);

            // Verificar se a lista retornada não é nula
            Assert.NotNull(transacoesRetornadas);
            // Verificar se o número de transações retornadas é o mesmo que o esperado
            Assert.Equal(transacoesEsperadas.Count, transacoesRetornadas.Count());
        }
    }
}