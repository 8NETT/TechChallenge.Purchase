# Tech Challenge - Módulo de Compras

Este repositório contém o código-fonte do serviço de processamento de compras, uma API RESTful desenvolvida como parte do Tech Challenge da pós-graduação. O serviço utiliza uma arquitetura limpa (Clean Architecture) e é orientada a eventos para processar compras de forma assíncrona, escalável e resiliente.

## Estrutura do Projeto

A solução é baseada em uma arquitetura limpa para separar as responsabilidades e promover baixo acoplamento e alta coesão:

-   `Src/TechChallenge.Purchases.Web/`: Projeto principal da API (ASP.NET Core). Contém os Endpoints, configurações de injeção de dependência, middlewares e o ponto de entrada da aplicação.
-   `Src/TechChallenge.Purchases.Application/`: Camada de aplicação. Contém a lógica de negócio, serviços, DTOs, mappers e validações.
-   `Src/TechChallenge.Purchases.Core/`: O núcleo da aplicação. Contém as entidades de domínio, interfaces de repositório, exceções customizadas e objetos de valor.
-   `Src/TechChallenge.Purchases.Infrastructure/`: Camada de infraestrutura. Fornece as implementações concretas dos contratos definidos no Core, como repositórios (usando Entity Framework Core) e clientes de serviços externos (Azure Event Hub).
-   `TechChallenge.Purchases.Tests/`: Projeto de testes unitários, utilizando xUnit para garantir a qualidade e o comportamento esperado das funcionalidades.
-   `.github/`: Contém os workflows de CI/CD do GitHub Actions, que automatizam o build, teste e deploy da aplicação.
-   `Dockerfile`: Permite a containerização da aplicação, facilitando o deploy e a portabilidade entre ambientes.

## Tecnologias Utilizadas

-   **.NET 9**: A versão mais recente da plataforma de desenvolvimento da Microsoft, utilizada para construir uma aplicação performática e com recursos modernos.
-   **ASP.NET Core**: Framework para a construção de APIs web de alta performance.
-   **Entity Framework Core**: ORM utilizado para a comunicação com o banco de dados SQL Server.
-   **Azure Event Hubs**: Serviço de streaming de Big Data e ingestão de eventos, utilizado como o barramento de eventos para a comunicação assíncrona após a criação de uma compra.
-   **OpenTelemetry & New Relic**: Padrões de observabilidade utilizados para instrumentar a aplicação, coletando traces e métricas para monitoramento e análise de performance.
-   **xUnit**: Framework de testes unitários para a plataforma .NET, usado para criar e executar testes que validam a lógica da aplicação.
-   **Docker**: Plataforma de containerização utilizada para empacotar a aplicação e suas dependências. O `Dockerfile` no projeto utiliza um build multi-stage para criar uma imagem final otimizada e segura.
-   **GitHub Actions**: Automação de CI/CD integrada ao GitHub. O pipeline configurado realiza o build, executa os testes e, em caso de push para a branch `main`, publica a imagem Docker.

## Como Executar a Aplicação

### Pré-requisitos

-   [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
-   [Docker](https://www.docker.com/products/docker-desktop) (Opcional)
-   Um servidor SQL Server acessível.

### Usando o .NET CLI

1.  Clone o repositório:
    ```bash
    git clone https://github.com/seu-usuario/TechChallenge.Purchase.git
    cd TechChallenge.Purchase
    ```

2.  Crie o arquivo `appsettings.Development.json` na pasta `Src/TechChallenge.Purchases.Web/` e configure as conexões:
    ```json
    {
      "ConnectionStrings": {
        "ConnectionString": "Server=seu_servidor_sql;Database=purchaseDb;User Id=seu_usuario;Password=sua_senha;TrustServerCertificate=True;"
      },
      "Jwt": {
        "Key": "SuaChaveSecretaParaJwt",
        "Issuer": "SeuIssuer"
      },
      "EventHub": {
        "ConnectionString": "sua-connection-string-do-event-hub",
        "HubName": "nome-do-hub-de-compras"
      }
    }
    ```

3.  Execute a aplicação a partir da raiz do projeto:
    ```bash
    dotnet run --project Src/TechChallenge.Purchases.Web/
    ```
    A API estará disponível em `https://localhost:<porta>` e `http://localhost:<porta>`, e o Swagger em `https://localhost:<porta>/swagger`.

### Usando Docker

1.  Na raiz do projeto, construa a imagem Docker:
    ```bash
    docker build -t techchallenge-purchases .
    ```

2.  Execute o container. Você precisará passar as variáveis de ambiente necessárias para a conexão com o SQL Server e o Event Hub.
    ```bash
    docker run -p 8083:8083 \
           -e ConnectionStrings__ConnectionString="sua-connection-string-sql" \
           -e EventHub__ConnectionString="sua-connection-string-eventhub" \
           -e EventHub__HubName="seu-hub" \
           -e Jwt__Key="sua-chave-jwt" \
           -e Jwt__Issuer="seu-issuer" \
           techchallenge-purchases
    ```
    A API estará escutando na porta `8083` do seu host.

## Como Executar os Testes

Para executar todos os testes da solução, navegue até a raiz do projeto e execute o seguinte comando:

```bash
dotnet test
```
