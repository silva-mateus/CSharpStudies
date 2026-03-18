# Resumao - Prova Tecnica: Senior Software Development Engineer (Ascendon)

> Documento de referencia rapida para a prova tecnica. Foco em C#, .NET, ASP.NET Core, SQL, AWS e boas praticas do time Ascendon.

---

## 1. Sobre o Ascendon

O **Ascendon** e a plataforma **cloud-based SaaS BSS** (Business Support System) da CSG. Atende empresas de telecomunicacoes, entretenimento, esportes e TV por assinatura.

**O que faz:**
- Gerencia assinaturas, billing, pagamentos, catalogo de ofertas e suporte ao cliente
- Permite lancar novos servicos digitais rapidamente (em dias, nao meses)
- Unifica todos os servicos em um unico relacionamento com o cliente

**Caracteristicas arquiteturais:**
- **Multi-tenant SaaS** -- uma instancia compartilhada, dados isolados por tenant
- **Microservices** -- stateless, horizontally scalable
- **Event-driven** -- comunicacao assincrona entre componentes
- **Cloud-native** -- roda 100% na AWS
- **350+ open APIs** -- RESTful JSON alinhado com OpenAPI Spec (OAS) v3

### Tech Stack do Ascendon

```
Compute:        ECS Fargate, Lambda (serverless), EC2 (legado Windows)
APIs:           API Gateway, ALB (Application Load Balancer), NLB
Data Stores:    SQL Server, Aurora (RDS), DynamoDB, RDS MySQL, OpenSearch, S3
Cache:          ElastiCache (Redis)
Messaging:      SNS, SQS, Kinesis, EventBridge
Security:       IAM Roles, WAF, Parameter Store (secrets), JWT, OAuth2, SAML
Monitoring:     CloudWatch, Kinesis Firehose, Elasticsearch, Kibana, X-Ray
CI/CD:          Azure DevOps (pipelines), Terraform, Packer
Languages:      C# / .NET (backend), Angular / React (frontend)
Testing:        xUnit (unit tests), integration tests automatizados
```

---

## 2. Requisitos da Vaga (Mapeados)

| Requisito da JD | O que saber |
|---|---|
| C#, .NET | Generics, LINQ, async/await, records, pattern matching, DI |
| ASP.NET Core | Minimal APIs, middleware, validation, WebApplicationFactory |
| SQL / Schema Design | JOINs, CTEs, Window Functions, indexes, normalizacao |
| REST / Web Services | Verbos HTTP, status codes, paginacao, versionamento |
| xUnit | Fact, Theory, InlineData, FluentAssertions, mocking |
| AWS Cloud Services | Lambda, ECS, S3, DynamoDB, SQS, SNS, API Gateway |
| Terraform (IaC) | Conceitos basicos: providers, resources, modules, state |
| Git / CI/CD | Branching, PRs, code review, Azure DevOps pipelines |
| AI / ML | Familiaridade com uso de AI em dev (Copilot, code gen, etc.) |
| Telecom BSS | Billing, subscription management, order management (plus) |

---

## 3. Coding Standards do Ascendon

O time segue convencoes estritas. Codigo fora do padrao e rejeitado em **code review**.

### Naming Conventions

```csharp
// PascalCase: classes, methods, properties, public fields
public class ProductService { }
public string FirstName { get; set; }
public void CalculateTotal() { }

// camelCase com prefixo _: private fields
private readonly ILogger _logger;
private int _retryCount;

// s_ prefixo: static fields
private static int s_instanceCount;

// I prefixo: interfaces
public interface IProductRepository { }

// Allman style braces (chave na linha de baixo)
if (condition)
{
    DoSomething();
}

// Usar 'var' quando o tipo e obvio
var products = new List<Product>();
var name = "hello"; // OK, tipo obvio

// Named parameters para clareza
DoSomething(foo: "someString", bar: 1);
```

### Exception Handling

```csharp
// CORRETO: preserva stack trace
try { /* ... */ }
catch (Exception ex)
{
    _logger.LogError(ex, "Erro ao processar pedido {OrderId}", orderId);
    throw; // <-- SEMPRE usar throw; (sem ex)
}

// ERRADO: perde stack trace
catch (Exception ex) { throw ex; }           // NUNCA
catch (Exception ex) { throw new Exception(ex.Message); } // NUNCA
```

### Logging com ILogger

```csharp
// Structured logging com placeholders (nao string interpolation!)
_logger.LogInformation("Processando pedido {OrderId} para usuario {UserId}", orderId, userId);
_logger.LogDebug("Payload recebido: {@Request}", request);
_logger.LogError(ex, "Falha ao processar pedido {OrderId}", orderId);

// Regras:
// - LogInformation: entrada/saida de metodos
// - LogDebug: payloads, diagnosticos
// - LogError: exceptions
// - SEMPRE incluir correlation IDs (requestId, userId, orderId)
// - NUNCA logar dados sensiveis (passwords, tokens, PII)
```

### Async/Await

```csharp
// Chamadas independentes em paralelo com Task.WhenAll
var customerTask = GetCustomerAsync(id);
var ordersTask = GetOrdersAsync(id);
await Task.WhenAll(customerTask, ordersTask);
var customer = customerTask.Result;
var orders = ordersTask.Result;

// NUNCA bloquear com .Result ou .Wait() em codigo sincrono
// SEMPRE propagar CancellationToken
public async Task<Product> GetByIdAsync(Guid id, CancellationToken ct = default)
{
    return await _dbContext.Products.FindAsync(id, ct);
}
```

### Principios Obrigatorios

- **SOLID** -- seguir rigorosamente (ver secao 6)
- **DRY** -- nao duplicar codigo
- **Secrets** -- usar AWS Parameter Store, nunca hardcodar valores de ambiente
- **NuGet** -- compartilhar modulos via NuGet feeds internos

---

## 4. C# Quick Reference

### Records e Init-Only Properties

```csharp
// Record: imutavel, value equality, com deconstruction
public record ProductFilter(
    string? Category,
    decimal? MinPrice,
    decimal? MaxPrice,
    string? Search,
    int Page = 1,
    int PageSize = 10);

// Record class vs record struct
public record class CustomerDto(string Name, string Email);
public readonly record struct Point(int X, int Y);
```

### Pattern Matching

```csharp
// Switch expression
var discount = customer.Tier switch
{
    "Gold" => 0.2m,
    "Silver" => 0.1m,
    "Bronze" => 0.05m,
    _ => 0m
};

// Property pattern
if (order is { Status: "Pending", Total: > 100 })
    ApplyDiscount(order);

// List pattern (C# 11+)
int[] numbers = { 1, 2, 3 };
var result = numbers is [1, _, 3]; // true
```

### Null Handling

```csharp
// Null-coalescing
var name = input ?? "default";

// Null-coalescing assignment
list ??= new List<string>();

// Null-conditional
var length = name?.Length ?? 0;

// Required (C# 11+)
public required string Name { get; init; }
```

### LINQ Essencial

```csharp
// Fluent syntax -- preferido pelo time
var topProducts = products
    .Where(p => p.Price > 10)
    .OrderByDescending(p => p.Price)
    .GroupBy(p => p.Category)
    .Select(g => new { Category = g.Key, Count = g.Count(), Avg = g.Average(p => p.Price) })
    .Take(5);

// Metodos uteis para lembrar
.Any(), .All(), .First(), .FirstOrDefault(), .Single(), .SingleOrDefault()
.Distinct(), .SelectMany(), .Zip(), .Aggregate()
.ToDictionary(), .ToLookup(), .ToHashSet()
```

### Generics com Constraints

```csharp
public class Repository<T> where T : class, IEntity, new()
{
    public T Create() => new T();
}

// Constraints disponiveis:
// where T : class          (reference type)
// where T : struct         (value type)
// where T : new()          (parameterless constructor)
// where T : IInterface     (implementa interface)
// where T : BaseClass      (herda de classe)
// where T : notnull        (non-nullable)
```

### Dependency Injection Lifecycles

```csharp
builder.Services.AddSingleton<ICache, RedisCache>();     // 1 instancia para toda app
builder.Services.AddScoped<IDbContext, AppDbContext>();   // 1 instancia por request
builder.Services.AddTransient<IValidator, Validator>();   // nova instancia toda vez

// Registrar todos os validators de um assembly
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductRequestValidator>();
```

---

## 5. Design Patterns Essenciais

### Repository Pattern

```csharp
public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id);
    Task<IEnumerable<Product>> GetAllAsync(ProductFilter filter);
    Task<Product> CreateAsync(Product product);
    Task<Product?> UpdateAsync(Guid id, Product product);
    Task<bool> DeleteAsync(Guid id);
}

public class SqlProductRepository : IProductRepository
{
    private readonly AppDbContext _context;
    public SqlProductRepository(AppDbContext context) => _context = context;

    public async Task<Product?> GetByIdAsync(Guid id)
        => await _context.Products.FindAsync(id);
}
```

### Strategy Pattern

```csharp
public interface IPaymentStrategy
{
    Task<PaymentResult> ProcessAsync(PaymentRequest request);
}

public class CreditCardPayment : IPaymentStrategy { /* ... */ }
public class PixPayment : IPaymentStrategy { /* ... */ }

public class PaymentProcessor
{
    private readonly IEnumerable<IPaymentStrategy> _strategies;

    public PaymentProcessor(IEnumerable<IPaymentStrategy> strategies)
        => _strategies = strategies;

    public async Task<PaymentResult> ProcessAsync(string method, PaymentRequest request)
    {
        var strategy = _strategies.FirstOrDefault(s => s.GetType().Name.Contains(method))
            ?? throw new NotSupportedException($"Payment method '{method}' not supported");
        return await strategy.ProcessAsync(request);
    }
}
```

### Retry Policy / Circuit Breaker

```csharp
// Conceito: retry com exponential backoff
public async Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> action, int maxRetries = 3)
{
    for (int i = 0; i <= maxRetries; i++)
    {
        try { return await action(); }
        catch (Exception) when (i < maxRetries)
        {
            await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, i))); // 1s, 2s, 4s
        }
    }
    throw new InvalidOperationException("Unreachable");
}

// Circuit Breaker: abre circuito apos N falhas, espera cooldown, tenta novamente
// Estados: Closed (normal) -> Open (bloqueado) -> HalfOpen (testando)
```

---

## 6. SOLID - Resumo Rapido

| Principio | Regra | Exemplo pratico |
|---|---|---|
| **S**ingle Responsibility | Uma classe = uma responsabilidade | `OrderValidator` so valida, `OrderRepository` so persiste |
| **O**pen/Closed | Aberto para extensao, fechado para modificacao | Adicionar novo `IPaymentStrategy` sem alterar `PaymentProcessor` |
| **L**iskov Substitution | Subclasses substituem a base sem quebrar | `SqlRepo` e `InMemoryRepo` implementam `IRepository<T>` igualmente |
| **I**nterface Segregation | Interfaces pequenas e focadas | `IReadRepository<T>` e `IWriteRepository<T>` separados |
| **D**ependency Inversion | Depender de abstracoes, nao implementacoes | Injetar `IProductRepository`, nunca `SqlProductRepository` direto |

---

## 7. ASP.NET Core - Minimal APIs

### Estrutura Basica

```csharp
var builder = WebApplication.CreateBuilder(args);

// Registrar servicos
builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductRequestValidator>();

var app = builder.Build();

// Middleware de erro global
app.Use(async (context, next) =>
{
    try { await next(context); }
    catch (Exception ex)
    {
        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(new { title = "Internal error", status = 500 });
    }
});

// Endpoints
app.MapGet("/products", async (IProductRepository repo) => Results.Ok(await repo.GetAllAsync()));
app.MapGet("/products/{id:guid}", async (Guid id, IProductRepository repo) =>
    await repo.GetByIdAsync(id) is { } product ? Results.Ok(product) : Results.NotFound());
app.MapPost("/products", async (CreateProductRequest req, IValidator<CreateProductRequest> v, IProductRepository repo) =>
{
    var result = await v.ValidateAsync(req);
    if (!result.IsValid)
        return Results.ValidationProblem(result.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray()));
    var product = await repo.CreateAsync(new Product { Name = req.Name, Price = req.Price });
    return Results.Created($"/products/{product.Id}", product);
});

app.Run();
public partial class Program { } // necessario para WebApplicationFactory
```

### Results Helper Methods

```csharp
Results.Ok(value)                    // 200
Results.Created(uri, value)          // 201
Results.NoContent()                  // 204
Results.BadRequest(value)            // 400
Results.NotFound()                   // 404
Results.ValidationProblem(errors)    // 400 com detalhes de validacao
Results.Problem(detail, statusCode)  // RFC 7807 problem details
```

### FluentValidation

```csharp
public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome e obrigatorio")
            .MaximumLength(200);

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Preco deve ser positivo");

        RuleFor(x => x.Category)
            .NotEmpty()
            .Must(c => ValidCategories.Contains(c))
            .WithMessage("Categoria invalida");
    }
}
```

---

## 8. Testing com xUnit

### Fact vs Theory

```csharp
// Fact: teste sem parametros
[Fact]
public void Add_TwoNumbers_ReturnsSum()
{
    var result = Calculator.Add(2, 3);
    result.Should().Be(5);
}

// Theory + InlineData: teste parametrizado
[Theory]
[InlineData(1, 2, 3)]
[InlineData(-1, 1, 0)]
[InlineData(0, 0, 0)]
public void Add_VariousInputs_ReturnsExpected(int a, int b, int expected)
{
    Calculator.Add(a, b).Should().Be(expected);
}

// MemberData: dados de um metodo/property
[Theory]
[MemberData(nameof(GetTestCases))]
public void Process_WithTestData_Works(string input, string expected) { /* ... */ }

public static IEnumerable<object[]> GetTestCases()
{
    yield return new object[] { "abc", "ABC" };
    yield return new object[] { "", "" };
}
```

### FluentAssertions

```csharp
// Valores
result.Should().Be(5);
result.Should().BeGreaterThan(0);
result.Should().BeInRange(1, 10);

// Strings
name.Should().StartWith("Jo").And.EndWith("hn");
name.Should().Contain("oh");
name.Should().MatchRegex(@"^\d+$");

// Collections
list.Should().HaveCount(3);
list.Should().Contain(x => x.Price > 10);
list.Should().BeInAscendingOrder(x => x.Name);
list.Should().OnlyContain(x => x.IsActive);

// Exceptions
act.Should().Throw<ArgumentException>().WithMessage("*invalid*");
await act.Should().ThrowAsync<HttpRequestException>();

// Objects
product.Should().BeEquivalentTo(expected, opt => opt.Excluding(p => p.Id));
```

### Integration Tests com WebApplicationFactory

```csharp
public class ProductApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ProductApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetProducts_ReturnsOk()
    {
        var response = await _client.GetAsync("/products");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<PagedResponse<Product>>();
        content.Should().NotBeNull();
        content!.Items.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CreateProduct_WithInvalidData_ReturnsValidationError()
    {
        var request = new { Name = "", Price = -1 };
        var response = await _client.PostAsJsonAsync("/products", request);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
```

### Arrange-Act-Assert Pattern

```csharp
[Fact]
public async Task GetById_ExistingProduct_ReturnsProduct()
{
    // Arrange
    var repo = new InMemoryProductRepository();
    var product = await repo.CreateAsync(new Product { Name = "Test", Price = 9.99m });

    // Act
    var result = await repo.GetByIdAsync(product.Id);

    // Assert
    result.Should().NotBeNull();
    result!.Name.Should().Be("Test");
}
```

---

## 9. SQL / Data Access

### SQL - Conceitos Importantes

```sql
-- JOIN: combinar tabelas
SELECT o.Id, c.Name, o.Total
FROM Orders o
INNER JOIN Customers c ON o.CustomerId = c.Id
LEFT JOIN OrderItems oi ON o.Id = oi.OrderId;

-- CTE (Common Table Expression): consultas complexas legiveisv
WITH MonthlySales AS (
    SELECT CustomerId, SUM(Total) AS TotalSpent,
           RANK() OVER (ORDER BY SUM(Total) DESC) AS Ranking
    FROM Orders
    WHERE OrderDate >= DATEADD(MONTH, -1, GETDATE())
    GROUP BY CustomerId
)
SELECT c.Name, ms.TotalSpent, ms.Ranking
FROM MonthlySales ms
JOIN Customers c ON ms.CustomerId = c.Id
WHERE ms.Ranking <= 10;

-- Window Functions
SELECT Name, Category, Price,
       ROW_NUMBER() OVER (PARTITION BY Category ORDER BY Price DESC) AS RowNum,
       AVG(Price) OVER (PARTITION BY Category) AS AvgCategoryPrice
FROM Products;

-- PIVOT: transformar linhas em colunas
SELECT * FROM (
    SELECT Category, MONTH(OrderDate) AS Mes, Total
    FROM Orders
) src
PIVOT (SUM(Total) FOR Mes IN ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])) pvt;
```

### Entity Framework Core

```csharp
// DbContext
public class AppDbContext : DbContext
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Fluent API configuration
        modelBuilder.Entity<Product>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Name).IsRequired().HasMaxLength(200);
            e.Property(p => p.Price).HasPrecision(18, 2);
            e.HasIndex(p => p.Category);
        });

        modelBuilder.Entity<Order>(e =>
        {
            e.HasMany(o => o.Items)
             .WithOne(i => i.Order)
             .HasForeignKey(i => i.OrderId)
             .OnDelete(DeleteBehavior.Cascade);
        });
    }
}

// Repository com EF Core
public class EfProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public async Task<PagedResult<Product>> GetAllAsync(ProductFilter filter)
    {
        var query = _context.Products.AsQueryable();

        if (!string.IsNullOrEmpty(filter.Category))
            query = query.Where(p => p.Category == filter.Category);
        if (filter.MinPrice.HasValue)
            query = query.Where(p => p.Price >= filter.MinPrice);

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderBy(p => p.Name)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return new PagedResult<Product>(items, totalCount);
    }
}
```

### Dapper (micro-ORM)

```csharp
public class DapperProductRepository
{
    private readonly IDbConnection _connection;

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        const string sql = "SELECT * FROM Products WHERE Id = @Id";
        return await _connection.QuerySingleOrDefaultAsync<Product>(sql, new { Id = id });
    }

    public async Task<IEnumerable<Product>> GetByCategoryAsync(string category)
    {
        const string sql = "SELECT * FROM Products WHERE Category = @Category ORDER BY Name";
        return await _connection.QueryAsync<Product>(sql, new { Category = category });
    }
}
```

---

## 10. AWS Quick Reference

| Servico | O que e | Quando usar |
|---|---|---|
| **Lambda** | Funcoes serverless, pagamento por execucao | Processamento event-driven, microservices leves |
| **ECS Fargate** | Containers gerenciados sem servidor | Microservices com mais controle, workloads longas |
| **API Gateway** | Gerenciamento de APIs RESTful | Expor APIs publicas com throttling, auth, caching |
| **DynamoDB** | Banco NoSQL key-value, serverless | Alta performance, schema flexivel, escala automatica |
| **RDS / Aurora** | Bancos relacionais gerenciados (SQL Server, MySQL, PostgreSQL) | Dados transacionais, relacoes complexas |
| **S3** | Object storage ilimitado | Arquivos, backups, data lake, static hosting |
| **SQS** | Fila de mensagens (queue) | Desacoplar produtores e consumidores, garantir entrega |
| **SNS** | Pub/Sub notifications | Fan-out de eventos para multiplos subscribers |
| **EventBridge** | Event bus serverless | Roteamento de eventos entre servicos com regras |
| **Kinesis** | Streaming de dados em tempo real | Logs, metricas, analytics em tempo real |
| **ElastiCache** | Cache gerenciado (Redis/Memcached) | Cache de sessoes, dados frequentemente acessados |
| **Parameter Store** | Armazenamento seguro de config/secrets | Connection strings, API keys, feature flags |
| **CloudWatch** | Monitoring, logs, alertas | Observabilidade da aplicacao |
| **X-Ray** | Distributed tracing | Rastrear requests atraves de microservices |
| **IAM** | Controle de acesso (roles, policies) | Seguranca entre servicos, least privilege |
| **WAF** | Web Application Firewall | Protecao contra SQL injection, XSS, DDoS |
| **Terraform** | Infrastructure as Code | Provisionar e gerenciar infraestrutura de forma reproduzivel |

---

## 11. Refactoring Checklist (Para a Prova)

### Passo a Passo ao Receber Codigo Legado

1. **Ler tudo primeiro** -- entender o que o codigo faz antes de mudar qualquer coisa
2. **Identificar code smells** -- ver lista abaixo
3. **Escrever characterization tests** -- testes que documentam o comportamento atual
4. **Refatorar em passos pequenos** -- cada passo deve manter os testes passando
5. **Aplicar patterns** -- Repository, Strategy, DI conforme necessario
6. **Validar** -- rodar todos os testes, verificar edge cases

### Red Flags para Procurar

```
[ ] God Class -- classe com muitas responsabilidades
[ ] Magic strings/numbers -- valores hardcoded sem constantes
[ ] Tight coupling -- classes instanciando dependencias diretamente (new)
[ ] No error handling -- try/catch ausente ou generico demais
[ ] No validation -- input do usuario nao validado
[ ] Duplicated code -- logica repetida em multiplos lugares
[ ] Long methods -- metodos com mais de ~20 linhas
[ ] Deep nesting -- if/else/for aninhados demais
[ ] No interfaces -- dependencias concretas, impossivel testar
[ ] No async -- operacoes I/O bloqueantes
[ ] Console.WriteLine -- em vez de ILogger
[ ] Static state -- estado compartilhado sem thread safety
```

### Tecnicas Comuns de Refactoring

| Tecnica | Quando usar |
|---|---|
| **Extract Method** | Metodo longo, logica repetida |
| **Extract Interface** | Dependencia concreta, precisa de DI |
| **Replace Conditional with Polymorphism** | Switch/if-else gigante para tipos |
| **Introduce Parameter Object** | Metodo com muitos parametros |
| **Replace Magic Number with Constant** | Valores hardcoded |
| **Move Method / Extract Class** | Classe com responsabilidades demais |
| **Replace new with DI** | Acoplamento forte a implementacoes |

---

## 12. Dicas para a Prova Tecnica

### Mentalidade

- **Leia tudo antes de comecar** -- entenda o escopo completo
- **Pergunte** -- se algo nao esta claro, pergunte ao entrevistador
- **Pense em voz alta** -- explique seu raciocinio enquanto codifica
- **Qualidade > Velocidade** -- melhor entregar menos, bem feito
- **Tests first** -- mostre que voce pensa em testabilidade

### Checklist de Entrega

```
[ ] Codigo compila e roda sem erros
[ ] Testes unitarios passando
[ ] Validation em todos os inputs
[ ] Error handling com mensagens claras
[ ] Logging estruturado onde relevante
[ ] Async/await em operacoes I/O
[ ] DI para todas as dependencias
[ ] Nomes claros e auto-explicativos
[ ] Sem codigo comentado ou morto
[ ] HTTP status codes corretos (200, 201, 204, 400, 404, 500)
```

### Frases Uteis para Explicar Decisoes

- *"Extraí essa interface para desacoplar e facilitar testes unitarios"*
- *"Usei o Strategy Pattern aqui porque temos multiplas variacoes do mesmo comportamento"*
- *"Apliquei FluentValidation para centralizar regras de validacao e mante-las testáveis"*
- *"Optei por async/await porque essa operacao envolve I/O (banco/rede)"*
- *"Criei um Repository para isolar o acesso a dados e respeitar o Single Responsibility"*
- *"Usei Task.WhenAll porque essas chamadas sao independentes e podem rodar em paralelo"*
- *"Guardei secrets no Parameter Store ao inves de hardcodar no codigo"*

---

## Bonus: Comandos Rapidos

### dotnet CLI

```bash
dotnet new webapi -n MeuProjeto        # criar projeto
dotnet add package FluentValidation    # adicionar pacote
dotnet build                           # compilar
dotnet test                            # rodar testes
dotnet run                             # executar
dotnet watch run                       # executar com hot reload
```

### Git (fluxo basico)

```bash
git checkout -b feature/minha-feature  # criar branch
git add .                              # stage changes
git commit -m "feat: add product API"  # commit
git push -u origin HEAD               # push
```

---

> **Boa sorte na prova!** Lembre-se: o entrevistador quer ver como voce pensa, nao apenas o resultado final. Mostre seu processo de raciocinio, faca perguntas e demonstre que voce se preocupa com qualidade e manutenibilidade.
