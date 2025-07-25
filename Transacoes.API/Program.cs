using Microsoft.Extensions.Options; // Importar para usar IOptions
using MongoDB.Driver;
using Transacoes.Application.Interfaces;
using Transacoes.Application.Services;
using Transacoes.Domain.Interfaces;
using Transacoes.Infrastructure.Repositories;
using Transacoes.Infrastructure.Settings;

var builder = WebApplication.CreateBuilder(args);

// --- Seção: Configuração da Injeção de Dependência ---

// 1. Carregar as configurações do banco
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

// 2. Registrar o IMongoClient 
// Registra o cliente MongoDB como um singleton. Uma única instância será usada durante a vida do app.
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

// 3. Registra o IMongoDatabase 
// Registra o banco de dados MongoDB como um singleton.
builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    var mongoClient = sp.GetRequiredService<IMongoClient>();
    return mongoClient.GetDatabase(settings.DatabaseName);
});

// uma nova instância para cada requisição HTTP.

// Registra a interface do Repositório (ITransacaoRepository) e sua implementação (TransacaoRepository)
builder.Services.AddScoped<ITransacaoRepository, TransacaoRepository>();

// Registra a interface do Serviço de Aplicação (ITransacaoService) e sua implementação (TransacaoService)
builder.Services.AddScoped<ITransacaoService, TransacaoService>();

// Configurações Padrão 
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

/*
// configuração do pipeline das requisições
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
