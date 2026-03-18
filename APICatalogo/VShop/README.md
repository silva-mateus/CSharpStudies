# VShop - Microservices com Clean Architecture

Projeto de estudo focado na construcao de uma aplicacao de e-commerce utilizando **microservicos** e **Clean Architecture** com .NET 9.

## Objetivo

Explorar na pratica os conceitos de:

- **Microservicos**: cada dominio de negocio (Produtos, Carrinho, Cupons) eh um servico independente com seu proprio banco de dados
- **Clean Architecture**: separacao em camadas (Domain, Application, Infrastructure, Api) com dependencias apontando para o centro
- **Minimal API**: endpoints leves e diretos, sem controllers
- **Autenticacao centralizada**: OAuth2/OpenID Connect com Duende IdentityServer
- **BFF (Backend For Frontend)**: o projeto Web consome as APIs e serve como frontend MVC

## Arquitetura

```
                    ┌──────────────┐
                    │   VShop.Web  │  (ASP.NET MVC - BFF)
                    └──────┬───────┘
                           │ HTTP
          ┌────────────────┼────────────────┐
          ▼                ▼                ▼
   ┌─────────────┐ ┌─────────────┐ ┌──────────────┐
   │ ProductApi  │ │   CartApi   │ │ DiscountApi  │
   │ :7016       │ │   :7287     │ │    :7003     │
   └─────────────┘ └─────────────┘ └──────────────┘
          │                │                │
          ▼                ▼                ▼
      [VShopDb]      [VShopCartDb]   [VShopDiscountDB]
                           │
                    ┌──────┴─────────┐
                    │ IdentityServer │  (OAuth2/OIDC - :7087)
                    └────────────────┘
```

## Clean Architecture por Microsservico

Cada API segue a mesma estrutura em 4 camadas, com dependencias unidirecionais:

```
Api  ──►  Application  ──►  Domain
 │              │
 └──►  Infrastructure  ──►  Domain
```

| Camada | Responsabilidade | Dependencias externas |
|---|---|---|
| **Domain** | Entidades, regras de negocio, interfaces de repositorio, excecoes tipadas | Nenhuma |
| **Application** | Casos de uso (UseCase), DTOs, mapeamentos (AutoMapper), interfaces de servicos externos | Domain |
| **Infrastructure** | Implementacoes de repositorio (EF Core + MySQL), DbContext, mensageria, DI extension | Domain, Application |
| **Api** | Endpoints (Minimal API), configuracao do host, autenticacao, OpenAPI | Application, Infrastructure |

### Exemplo: fluxo de uma requisicao

```
POST /api/products
      │
      ▼
ProductEndpoints.cs          (Api - recebe a requisicao)
      │
      ▼
CreateProductUseCase.cs      (Application - orquestra o caso de uso)
      │
      ├──► Product.Validate() (Domain - aplica regras de negocio)
      │
      ▼
ProductRepository.cs         (Infrastructure - persiste no banco)
```

## Estrutura de Projetos

```
VShop/
│
├── Backend/
│   ├── ProductApi/
│   │   ├── VShop.ProductApi.Domain/           # Entidades: Product, Category
│   │   ├── VShop.ProductApi.Application/      # 11 use cases, DTOs, mappings
│   │   ├── VShop.ProductApi.Infrastructure/   # EF Core, MySQL, repositories
│   │   └── VShop.ProductApi.Api/              # Minimal API, JWT auth
│   │
│   ├── CartApi/
│   │   ├── VShop.CartApi.Domain/              # Entidades: Cart, CartHeader, CartItem
│   │   ├── VShop.CartApi.Application/         # 6 use cases, IMessageProducer
│   │   ├── VShop.CartApi.Infrastructure/      # Repositories, NullMessageProducer
│   │   └── VShop.CartApi.Api/                 # Minimal API endpoints
│   │
│   ├── DiscountApi/
│   │   ├── VShop.DiscountApi.Domain/          # Entidade: Coupon
│   │   ├── VShop.DiscountApi.Application/     # 5 use cases CRUD + GetByCode
│   │   ├── VShop.DiscountApi.Infrastructure/  # EF Core, MySQL, CouponRepository
│   │   └── VShop.DiscountApi.Api/             # Minimal API endpoints
│   │
│   └── VShop.IdentityServer/                  # Duende IdentityServer (OAuth2/OIDC)
│
├── Frontend/
│   └── VShop.Web/                             # ASP.NET MVC (BFF para as APIs)
│
├── Tests/
│   ├── VShop.ProductApi.Domain.Tests/         # Testes unitarios de dominio
│   ├── VShop.ProductApi.Api.Tests/            # Testes de integracao (WebApplicationFactory)
│   ├── VShop.CartApi.Domain.Tests/
│   ├── VShop.CartApi.Api.Tests/
│   ├── VShop.DiscountApi.Domain.Tests/
│   └── VShop.DiscountApi.Api.Tests/
│
└── VShop.slnx
```

## Tecnologias

- **.NET 9** / C# 13
- **Minimal API** (endpoints sem controllers)
- **Entity Framework Core 9** + Pomelo MySQL
- **AutoMapper** (mapeamento Entity <-> DTO)
- **Duende IdentityServer** (autenticacao OAuth2/OIDC)
- **JWT Bearer** (validacao de tokens nas APIs)
- **Scalar** (documentacao interativa da API, alternativa ao Swagger UI)
- **xUnit** (testes unitarios e de integracao)
- **WebApplicationFactory** (testes de integracao com servidor in-memory)

## Conceitos Estudados

### Clean Architecture
- Separacao de responsabilidades em camadas com dependencias apontando para o dominio
- Entidades ricas com validacao de dominio (`Product.Validate()`, `Coupon.Validate()`)
- Interfaces de repositorio definidas no Domain, implementadas na Infrastructure
- Casos de uso como classes independentes com metodo `ExecuteAsync()`
- Inversao de dependencia via DI extensions (`AddInfrastructure()`, `AddApplication()`)

### Microservicos
- Cada servico possui seu proprio banco de dados (Database per Service)
- Comunicacao via HTTP entre o BFF (Web) e os servicos
- Autenticacao centralizada com IdentityServer
- Independencia de deploy (cada API eh um projeto executavel separado)

### Mensageria (Abstracao)
- Interface `IMessageProducer` definida na Application do CartApi
- Implementacao `NullMessageProducer` (no-op) na Infrastructure
- Preparado para substituicao futura por RabbitMQ sem alterar a logica de negocio

### Testes
- **Unitarios**: validam regras de dominio isoladamente (28 testes)
- **Integracao**: validam endpoints HTTP completos usando banco in-memory e autenticacao fake

## Como Executar

### Pre-requisitos
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- MySQL (local ou container)

### Configuracao

Cada API possui seu `appsettings.json` com a connection string do MySQL. Ajuste conforme seu ambiente:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=VShopDb;Uid=root;Pwd=SUA_SENHA"
  }
}
```

### Executar

```bash
# Restaurar dependencias
dotnet restore VShop.slnx

# Executar o IdentityServer (necessario para autenticacao)
dotnet run --project VShop.IdentityServer

# Em terminais separados, executar cada API
dotnet run --project VShop.ProductApi.Api
dotnet run --project VShop.CartApi.Api
dotnet run --project VShop.DiscountApi.Api

# Executar o frontend
dotnet run --project VShop.Web

# Executar testes
dotnet test VShop.slnx
```

### Portas

| Servico | HTTP | HTTPS |
|---|---|---|
| IdentityServer | - | https://localhost:7087 |
| ProductApi | http://localhost:5053 | https://localhost:7016 |
| CartApi | http://localhost:5056 | https://localhost:7287 |
| DiscountApi | http://localhost:5256 | https://localhost:7003 |
| Web | http://localhost:5144 | https://localhost:7068 |
