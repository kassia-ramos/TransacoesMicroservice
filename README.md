# Microsserviço de Gerenciamento de Transações Financeiras

## Descrição do Projeto
Este microsserviço, desenvolvido em C# com ASP.NET Core, tem como objetivo gerenciar transações financeiras, permitindo a criação e listagem de transações via API REST. Ele utiliza o MongoDB Atlas para persistência de dados e foi arquitetado seguindo os princípios da Clean Architecture, garantindo modularidade e testabilidade.

## Contexto do Desafio
Este projeto foi desenvolvido como parte do desafio Talent Lab 2025, com foco em Backend com C#. O objetivo é demonstrar proficiência na criação de APIs REST, integração com banco de dados NoSQL (MongoDB), uso de serviços de mensageria (Azure Service Bus) e aplicação de padrões de arquitetura e testes.

## Arquitetura
O projeto segue os princípios da **Clean Architecture**, dividindo a aplicação em camadas bem definidas para garantir a separação de preocupações, testabilidade e manutenibilidade:

* **Transacoes.Domain:** Contém as entidades de negócio (ex: `Transacao`) e interfaces de repositório (`ITransacaoRepository`). É o coração do sistema, independente de qualquer tecnologia externa.
* **Transacoes.Application:** Define as interfaces de serviço (`ITransacaoService`, `IMensagemServiceBusProdutor`) e implementa a lógica de aplicação (casos de uso), orquestrando as operações entre o Domínio e a Infraestrutura.
* **Transacoes.Infrastructure:** Responsável pela implementação de detalhes técnicos, como a persistência de dados no MongoDB Atlas (`TransacaoRepository`) e a integração com o Azure Service Bus (`ServiceBusProdutor`).
* **Transacoes.API:** A camada de apresentação, que expõe os endpoints REST (`TransacoesController`) e configura a Injeção de Dependência da aplicação.

## Tecnologias Utilizadas
* **C# (.NET 9.0):** Linguagem de programação e plataforma de desenvolvimento.
* **ASP.NET Core:** Framework para construção da API REST.
* **MongoDB Atlas:** Banco de dados NoSQL baseado em nuvem para armazenamento de transações.
* **MongoDB.Driver:** Driver oficial para integração C# com MongoDB.
* **Azure Service Bus:** Serviço de mensageria em nuvem para processamento assíncrono de transações.
* **Azure.Messaging.ServiceBus:** SDK oficial para integração C# com Azure Service Bus.
* **Swashbuckle.AspNetCore:** Para geração da documentação interativa da API (Swagger UI).
* **xUnit:** Framework de testes unitários.
* **Moq:** Biblioteca para criação de objetos simulados (mocks) em testes unitários.

## Como Configurar e Executar o Projeto

### Pré-requisitos
* [.NET SDK 9.0](https://dotnet.microsoft.com/download/dotnet/9.0) ou superior.
* Conta no [MongoDB Atlas](https://cloud.mongodb.com/) para configurar o banco de dados.
* Conta no [Azure Portal](https://portal.azure.com/) (preferencialmente com uma Assinatura de Educação para evitar gastos) para configurar o Azure Service Bus.

### Passos de Configuração

1.  **Clonar o Repositório:**
    ```bash
    git clone https://github.com/kassia-ramos/TransacoesMicroservice.git
    cd TransacoesMicroservice
    ```
2.  **Configurar Credenciais (Desenvolvimento Local):**
    * **Importante:** Este projeto utiliza `appsettings.Development.json` para armazenar credenciais sensíveis (MongoDB e Azure Service Bus) localmente, para garantir a não exposição dos dados.
    * Crie ou edite o arquivo `appsettings.Development.json` no projeto Transacao.API, e adicione as seguintes seções, substituindo os placeholders pelas suas credenciais reais:
        ```json
        {
          "Logging": {
            "LogLevel": {
              "Default": "Information",
              "Microsoft.AspNetCore": "Warning"
            }
          },

          "MongoDbSettings": {
            "ConnectionString": "SUA CHAVE DE ACESSO DO MONGO",
            "DatabaseName": "nome do banco criado por voce",
            "CollectionName": "nome da colecao criada por voce"
          },

          "AzureServiceBusSettings": {
            "ConnectionString": "SUA CHAVE DE ACESSO DA AZURE ",
            "QueueName": "nome-da-sua-fila"
          }
        }
        ```
        * **Para MongoDB Atlas:**
            * Crie um cluster (M0 Sandbox é suficiente).
            * Crie um usuário de banco de dados com permissões de leitura/escrita.
            * Configure o Acesso à Rede para permitir conexões do seu endereço IP.
            * Obtenha a **Connection String** e cole em `SUA CHAVE DE ACESSO DO MONGO` (a senha da Connection String).
        * **Para Azure Service Bus:**
            * Crie um Service Bus Namespace (Tipo de Preço: Basic ou Standard).
            * Crie uma Fila dentro do Namespace ( `nome-da-sua-fila`).
            * No Namespace, vá em "Políticas de acesso compartilhado" -> "RootManageSharedAccessKey".
            * Copie a **"Cadeia de conexão primária"** e cole em `SUA CHAVE DE ACESSO AZURE`.
            * Atualize `nome-da-sua-fila` com o nome exato da sua fila.

### Executando a Aplicação

1.  **Restaurar Dependências e Compilar:**
    Abra o terminal na raiz da solução (`TransacoesMicroservice`) e execute:
    ```bash
    dotnet restore
    dotnet build
    ```
2.  **Iniciar o Serviço (em Ambiente de Desenvolvimento):**
    Navegue para o projeto da API e inicie-o, garantindo o ambiente de `Development`:
    ```bash
    cd Transacoes.API
    dotnet run --launch-profile http
    ```
    Aguarde a mensagem `Application started. Now listening on: http://localhost:5008`.

### Endpoints da API (Swagger UI)

Após a aplicação iniciar, acesse a documentação interativa da API (Swagger UI) no navegador através do endereço:

* **URL:** `http://localhost:5008/swagger`

Você encontrará os seguintes endpoints:

* **`POST /api/Transacoes`**: Que Cria uma nova transação e envia uma mensagem para a fila do Azure Service Bus.
    * **Exemplo de Corpo da Requisição (JSON):**
        ```json
        {
          "valor": 150.00,
          "moeda": "BRL",
          "descricao": "Compra de materiais de escritório",
          "data": "2025-07-26T13:00:00Z"
        }
        ```
    * **Respostas:** Retorna `201 Created` com a transação criada e seu `Id` gerado pelo MongoDB, ou `400 Bad Request` em caso de erro de validação.

* **`GET /api/Transacoes`**: Lista todas as transações cadastradas.
    * **Respostas:** Retorna `200 OK` com uma lista de transações, ou `404 Not Found` se nenhuma transação for encontrada.


### Verificar as Transações no MongoDB e na Azure:

Após o cadastro e verificação pelas endpoints, você pode abrir novamente o seu perfil no MongoDB e Azure para verificar a lista de transações por ele.

Caso estejam abertos, apenas procure o cluster correspondente no MongoDB Atlas, e verifique as "Collections", lá listará as transações enviadas.
No Azure verifique sua lista e a chegada de mensagens correspondentes na lista.

## Testes Unitários

O projeto inclui **2 testes unitários** para a camada de `Transacoes.Application` (especificamente para o `TransacaoService`), implementados com **xUnit** e **Moq**.

Esses testes garantem que:
* A lógica de orquestração do `TransacaoService` funciona como esperado (chamar o repositório para salvar e o produtor de mensagens para enviar).
* A arquitetura permite o isolamento das unidades de código, possibilitando testá-las sem depender de serviços externos (MongoDB ou Azure Service Bus reais), usando mocks.

### Como Executar os Testes

1.  Abra o terminal na raiz da solução (`TransacoesMicroservice`).
2.  Execute o comando:
    ```bash
    dotnet test Transacoes.Tests.Unit/Transacoes.Tests.Unit.csproj
    ```
    O resultado esperado é "total: 2; falhou: 0; bem-sucedido: 2".

## Desafios Encontrados

Durante o desenvolvimento e integração com o Azure Service Bus, foi encontrado um desafio técnico persistente relacionado à autenticação. Embora o código de integração esteja completamente implementado e siga os princípios da Clean Architecture (com o `ServiceBusProdutor` delegando o envio de mensagens), em tempo de execução, a aplicação retornou repetidamente um erro `System.UnauthorizedAccessException: ExpiredToken`.

Apesar de múltiplas tentativas de depuração, que incluíram:
* Regeneração de Connection Strings no Azure Portal.
* Criação de novos Namespaces e Filas (com diferentes tipos de preço).
* Verificação exaustiva de permissões de acesso (RootManageSharedAccessKey, permissões 'Manage' e 'Send').
* Configurações de rede ('Todas as redes' permitidas).
* Limpeza e restauração de caches do .NET.
* Movimentação de credenciais para `appsettings.Development.json` para segurança.

O erro de token expirado persistiu, sugerindo que o problema está fora do escopo do código da aplicação. Isso pode estar relacionado a políticas de tempo de vida de token específicas da assinatura 'Azure for Students', ou a algum comportamento inesperado do serviço do Azure em determinada região/momento. A aplicação continua funcional para as operações de CRUD com MongoDB, e a lógica de envio para o Service Bus está presente e demonstrada nos testes unitários.

## Próximos Passos e Melhorias Potenciais
* Investigar e resolver a fundo o problema de autenticação do Azure Service Bus (com suporte Azure, se necessário).
* Implementar a leitura de mensagens da fila do Azure Service Bus por outro microsserviço ou função (processamento assíncrono).
* Adicionar validação mais sofisticada (FluentValidation) e tratamento global de erros (middleware) na API.
* Implementar Autenticação e Autorização (ex: JWT) para proteger os endpoints da API.
* Expandir a cobertura de testes unitários e adicionar testes de integração.

---
