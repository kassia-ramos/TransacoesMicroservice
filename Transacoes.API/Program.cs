using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Transacoes.Application.Interfaces;
using Transacoes.Application.Services;
using Transacoes.Domain.Interfaces;
using Transacoes.Infrastructure.Repositories;
using Transacoes.Infrastructure.Settings;
using Azure.Messaging.ServiceBus;
// Adicionado este using para a nova implementa��o do produtor de mensagens na camada Infrastructure:
using Transacoes.Infrastructure.Services; // Certifique-se que esta � a pasta correta (Services ou Producers)

var builder = WebApplication.CreateBuilder(args);


// Carregar as configura��es do banco
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

// Registrar o IMongoClient
// Registra o cliente MongoDB como um singleton. Uma �nica inst�ncia ser� usada durante a vida do app.
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

// Registra o IMongoDatabase
// Registra o banco de dados MongoDB como um singleton.
builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    var mongoClient = sp.GetRequiredService<IMongoClient>();
    return mongoClient.GetDatabase(settings.DatabaseName);
});

//carregar configuracoes do azure
builder.Services.Configure<AzureServiceBusSettings>(builder.Configuration.GetSection("AzureServiceBusSettings"));

// registrar o serviceBusClient como singleton
builder.Services.AddSingleton<ServiceBusClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<AzureServiceBusSettings>>().Value;
    return new ServiceBusClient(settings.ConnectionString);
});


// uma nova inst�ncia para cada requisi��o HTTP.

// Registra a interface do Reposit�rio (ITransacaoRepository) e sua implementa��o (TransacaoRepository)
builder.Services.AddScoped<ITransacaoRepository, TransacaoRepository>();

// NOVO REGISTRO: Registra a interface do Produtor de Mensagens do Service Bus e sua implementa��o
builder.Services.AddScoped<IMensagemServiceBusProdutor, ServiceBusProdutor>(); 

// Registra a interface do Servi�o de Aplica��o (ITransacaoService) e sua implementa��o (TransacaoService)
// O TransacaoService agora injeta IMensagemServiceProdutor
builder.Services.AddScoped<ITransacaoService, TransacaoService>(); // Esta linha permanece

// Configura��es Padr�o
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

/*
// configura��o do pipeline das requisi��es
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
*/

// TEMPORARIO para testes -
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();