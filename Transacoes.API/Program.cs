using Microsoft.Extensions.Options; // Importar para usar IOptions
using MongoDB.Driver;
using Transacoes.Application.Interfaces;
using Transacoes.Application.Services;
using Transacoes.Domain.Interfaces;
using Transacoes.Infrastructure.Repositories;
using Transacoes.Infrastructure.Settings;

var builder = WebApplication.CreateBuilder(args);

// --- Se��o: Configura��o da Inje��o de Depend�ncia ---

// 1. Carregar as configura��es do banco
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

// 2. Registrar o IMongoClient 
// Registra o cliente MongoDB como um singleton. Uma �nica inst�ncia ser� usada durante a vida do app.
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

// uma nova inst�ncia para cada requisi��o HTTP.

// Registra a interface do Reposit�rio (ITransacaoRepository) e sua implementa��o (TransacaoRepository)
builder.Services.AddScoped<ITransacaoRepository, TransacaoRepository>();

// Registra a interface do Servi�o de Aplica��o (ITransacaoService) e sua implementa��o (TransacaoService)
builder.Services.AddScoped<ITransacaoService, TransacaoService>();

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
