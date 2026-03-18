# Red Flags & Checklist de Entrega -- Exemplos Praticos

> Exemplos concretos de codigo "antes e depois" para cada red flag e item do checklist de entrega.
> Use como referencia rapida durante a prova tecnica.

---

## Parte 1: Red Flags (Codigo Ruim → Codigo Bom)

---

### 1. God Class

Uma classe que faz tudo: valida, salva no banco, envia email, gera relatorio.

**RUIM:**

```csharp
public class OrderService
{
    public void ProcessOrder(string customerName, string email, string product, decimal price, int qty)
    {
        // Validacao
        if (string.IsNullOrEmpty(customerName)) throw new Exception("Name required");
        if (price <= 0) throw new Exception("Invalid price");
        if (qty <= 0) throw new Exception("Invalid qty");

        // Calculo
        var total = price * qty;
        if (total > 1000) total *= 0.9m; // 10% discount
        var tax = total * 0.15m;
        var finalTotal = total + tax;

        // Salva no banco
        var connection = new SqlConnection("Server=localhost;Database=Orders;...");
        connection.Open();
        var cmd = new SqlCommand($"INSERT INTO Orders VALUES ('{customerName}', '{product}', {finalTotal})", connection);
        cmd.ExecuteNonQuery();
        connection.Close();

        // Envia email
        var smtp = new SmtpClient("smtp.company.com");
        smtp.Send("noreply@company.com", email, "Pedido confirmado",
            $"Ola {customerName}, seu pedido de {product} foi confirmado. Total: {finalTotal}");

        // Gera log
        Console.WriteLine($"Order processed for {customerName}, total: {finalTotal}");
    }
}
```

**BOM:**

```csharp
public class OrderService
{
    private readonly IOrderRepository _repository;
    private readonly IOrderValidator _validator;
    private readonly IPricingService _pricing;
    private readonly INotificationService _notification;
    private readonly ILogger<OrderService> _logger;

    public OrderService(
        IOrderRepository repository,
        IOrderValidator validator,
        IPricingService pricing,
        INotificationService notification,
        ILogger<OrderService> logger)
    {
        _repository = repository;
        _validator = validator;
        _pricing = pricing;
        _notification = notification;
        _logger = logger;
    }

    public async Task<Order> ProcessOrderAsync(CreateOrderRequest request)
    {
        _validator.Validate(request);

        var total = _pricing.Calculate(request.Price, request.Quantity);
        var order = new Order(request.CustomerName, request.Product, total);

        await _repository.SaveAsync(order);
        await _notification.SendOrderConfirmationAsync(order, request.Email);

        _logger.LogInformation("Pedido {OrderId} processado para {Customer}, total: {Total}",
            order.Id, request.CustomerName, total);

        return order;
    }
}
```

> **O que mudou:** Cada responsabilidade foi extraida para seu proprio servico (SRP). Tudo injetado via DI. Async para I/O. Logging estruturado.

---

### 2. Magic Strings / Numbers

Valores literais espalhados pelo codigo sem explicacao.

**RUIM:**

```csharp
public decimal CalculateShipping(decimal weight, string region)
{
    if (region == "SP")
        return weight * 2.5m;
    else if (region == "RJ")
        return weight * 3.0m;
    else if (region == "MG")
        return weight * 3.5m;
    else
        return weight * 5.0m;

    // O que significa 2.5? 3.0? Por que esses valores?
}

public void ProcessPayment(int status)
{
    if (status == 1) { /* approved */ }
    else if (status == 2) { /* declined */ }
    else if (status == 3) { /* pending */ }
}
```

**BOM:**

```csharp
public static class ShippingRates
{
    public const decimal SaoPaulo = 2.5m;
    public const decimal RioDeJaneiro = 3.0m;
    public const decimal MinasGerais = 3.5m;
    public const decimal Default = 5.0m;
}

public static class Regions
{
    public const string SaoPaulo = "SP";
    public const string RioDeJaneiro = "RJ";
    public const string MinasGerais = "MG";
}

public decimal CalculateShipping(decimal weight, string region)
{
    var rate = region switch
    {
        Regions.SaoPaulo => ShippingRates.SaoPaulo,
        Regions.RioDeJaneiro => ShippingRates.RioDeJaneiro,
        Regions.MinasGerais => ShippingRates.MinasGerais,
        _ => ShippingRates.Default
    };

    return weight * rate;
}

public enum PaymentStatus
{
    Approved = 1,
    Declined = 2,
    Pending = 3
}
```

> **O que mudou:** Constantes nomeadas, enum tipado, switch expression. O codigo se auto-documenta.

---

### 3. Tight Coupling

Classes criando dependencias diretamente com `new`, impossivel de testar ou substituir.

**RUIM:**

```csharp
public class ReportGenerator
{
    public string Generate(int orderId)
    {
        // Acoplado diretamente a SqlConnection e SmtpClient
        var db = new SqlConnection("Server=prod;Database=Orders;...");
        var orders = db.Query<Order>($"SELECT * FROM Orders WHERE Id = {orderId}");

        var report = $"Report for order {orderId}: {orders.First().Total}";

        var emailSender = new SmtpClient("smtp.company.com");
        emailSender.Send("noreply@co.com", "admin@co.com", "Report", report);

        return report;
    }
}
```

**BOM:**

```csharp
public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(int orderId);
}

public interface IEmailSender
{
    Task SendAsync(string to, string subject, string body);
}

public class ReportGenerator
{
    private readonly IOrderRepository _repository;
    private readonly IEmailSender _emailSender;

    public ReportGenerator(IOrderRepository repository, IEmailSender emailSender)
    {
        _repository = repository;
        _emailSender = emailSender;
    }

    public async Task<string> GenerateAsync(int orderId)
    {
        var order = await _repository.GetByIdAsync(orderId)
            ?? throw new KeyNotFoundException($"Order {orderId} not found");

        var report = $"Report for order {orderId}: {order.Total}";
        await _emailSender.SendAsync("admin@co.com", "Report", report);

        return report;
    }
}

// Registro no DI container
builder.Services.AddScoped<IOrderRepository, SqlOrderRepository>();
builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();
builder.Services.AddScoped<ReportGenerator>();
```

> **O que mudou:** Interfaces extraidas, DI via constructor, facil de mockar em testes, sem connection strings hardcoded.

---

### 4. No Error Handling

Sem try/catch, ou catch generico que engole a exception.

**RUIM:**

```csharp
public Product GetProduct(int id)
{
    // Nenhum tratamento -- explode em runtime
    var json = File.ReadAllText($"products/{id}.json");
    return JsonSerializer.Deserialize<Product>(json);
}

// Ou pior: catch que esconde o erro
public Product GetProductSilent(int id)
{
    try
    {
        var json = File.ReadAllText($"products/{id}.json");
        return JsonSerializer.Deserialize<Product>(json);
    }
    catch (Exception)
    {
        return null; // Erro silencioso -- quem chama nao sabe que falhou
    }
}
```

**BOM:**

```csharp
public async Task<Product> GetProductAsync(int id, CancellationToken ct = default)
{
    var path = Path.Combine(_productsDirectory, $"{id}.json");

    if (!File.Exists(path))
        throw new ProductNotFoundException(id);

    try
    {
        var json = await File.ReadAllTextAsync(path, ct);
        return JsonSerializer.Deserialize<Product>(json)
            ?? throw new InvalidDataException($"Failed to deserialize product {id}");
    }
    catch (JsonException ex)
    {
        _logger.LogError(ex, "Arquivo corrompido para produto {ProductId}", id);
        throw new InvalidDataException($"Product file for {id} contains invalid JSON", ex);
    }
}

public class ProductNotFoundException : Exception
{
    public int ProductId { get; }
    public ProductNotFoundException(int productId)
        : base($"Product with ID {productId} was not found")
    {
        ProductId = productId;
    }
}
```

> **O que mudou:** Exceptions especificas, mensagens claras, logging com contexto, custom exception tipada, async para I/O.

---

### 5. No Validation

Input do usuario vai direto para a logica de negocio sem nenhuma verificacao.

**RUIM:**

```csharp
app.MapPost("/products", (Product product, AppDbContext db) =>
{
    db.Products.Add(product);   // Nome vazio? Preco negativo? Tudo aceito!
    db.SaveChanges();
    return Results.Ok(product);
});
```

**BOM:**

```csharp
public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero")
            .LessThanOrEqualTo(99999.99m).WithMessage("Price exceeds maximum allowed");

        RuleFor(x => x.Category)
            .NotEmpty()
            .Must(c => AllowedCategories.Contains(c))
            .WithMessage("Invalid category. Allowed: {AllowedValues}");
    }

    private static readonly HashSet<string> AllowedCategories = new()
        { "Electronics", "Books", "Clothing", "Food" };
}

app.MapPost("/products", async (
    CreateProductRequest request,
    IValidator<CreateProductRequest> validator,
    IProductRepository repo) =>
{
    var validation = await validator.ValidateAsync(request);
    if (!validation.IsValid)
    {
        var errors = validation.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
        return Results.ValidationProblem(errors);
    }

    var product = new Product { Name = request.Name, Price = request.Price, Category = request.Category };
    var created = await repo.CreateAsync(product);
    return Results.Created($"/products/{created.Id}", created);
});
```

> **O que mudou:** FluentValidation com regras claras, mensagens customizadas, retorno de HTTP 400 com detalhes dos erros.

---

### 6. Duplicated Code

Logica identica copiada em multiplos lugares.

**RUIM:**

```csharp
public class OrderController
{
    public IResult CreateOrder(CreateOrderRequest request)
    {
        // Validacao duplicada em cada metodo
        if (string.IsNullOrEmpty(request.CustomerName))
            return Results.BadRequest("Customer name is required");
        if (request.Items == null || request.Items.Count == 0)
            return Results.BadRequest("At least one item is required");
        if (request.Items.Any(i => i.Quantity <= 0))
            return Results.BadRequest("Quantity must be positive");

        // ... logica
    }

    public IResult UpdateOrder(UpdateOrderRequest request)
    {
        // Mesma validacao copiada aqui!
        if (string.IsNullOrEmpty(request.CustomerName))
            return Results.BadRequest("Customer name is required");
        if (request.Items == null || request.Items.Count == 0)
            return Results.BadRequest("At least one item is required");
        if (request.Items.Any(i => i.Quantity <= 0))
            return Results.BadRequest("Quantity must be positive");

        // ... logica
    }
}
```

**BOM:**

```csharp
public class OrderRequestValidator : AbstractValidator<IOrderRequest>
{
    public OrderRequestValidator()
    {
        RuleFor(x => x.CustomerName).NotEmpty();
        RuleFor(x => x.Items).NotEmpty().WithMessage("At least one item is required");
        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.Quantity).GreaterThan(0);
        });
    }
}

// Uma unica validacao reutilizada em ambos os endpoints
private static async Task<IResult?> ValidateAsync<T>(T request, IValidator<T> validator)
{
    var result = await validator.ValidateAsync(request);
    if (!result.IsValid)
    {
        var errors = result.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
        return Results.ValidationProblem(errors);
    }
    return null;
}
```

> **O que mudou:** Validacao centralizada num Validator reutilizavel. Helper method para evitar repetir a logica de retorno de erros.

---

### 7. Long Methods

Metodo gigante com multiplas responsabilidades misturadas.

**RUIM:**

```csharp
public decimal ProcessInvoice(List<InvoiceItem> items, string customerType, string couponCode)
{
    decimal subtotal = 0;
    foreach (var item in items)
    {
        if (item.Type == "physical")
            subtotal += item.Price * item.Qty;
        else if (item.Type == "digital")
            subtotal += item.Price * item.Qty * 0.9m;
        else if (item.Type == "subscription")
            subtotal += item.Price * item.Qty * 0.85m;
    }

    decimal discount = 0;
    if (customerType == "gold") discount = subtotal * 0.2m;
    else if (customerType == "silver") discount = subtotal * 0.1m;
    else if (customerType == "bronze") discount = subtotal * 0.05m;

    if (couponCode == "SAVE10") discount += subtotal * 0.1m;
    else if (couponCode == "SAVE20") discount += subtotal * 0.2m;

    var afterDiscount = subtotal - discount;
    var tax = afterDiscount * 0.15m;
    var shipping = items.Any(i => i.Type == "physical") ? 9.99m : 0m;

    return afterDiscount + tax + shipping;
    // 25+ linhas, impossivel testar partes isoladas
}
```

**BOM:**

```csharp
public decimal ProcessInvoice(List<InvoiceItem> items, string customerType, string couponCode)
{
    var subtotal = CalculateSubtotal(items);
    var discount = CalculateDiscount(subtotal, customerType, couponCode);
    var tax = CalculateTax(subtotal - discount);
    var shipping = CalculateShipping(items);

    return subtotal - discount + tax + shipping;
}

private decimal CalculateSubtotal(List<InvoiceItem> items)
    => items.Sum(item => item.Price * item.Qty * GetTypeMultiplier(item.Type));

private static decimal GetTypeMultiplier(string type) => type switch
{
    "physical" => 1.0m,
    "digital" => 0.9m,
    "subscription" => 0.85m,
    _ => 1.0m
};

private decimal CalculateDiscount(decimal subtotal, string customerType, string couponCode)
{
    var tierDiscount = GetTierDiscount(customerType);
    var couponDiscount = GetCouponDiscount(couponCode);
    return subtotal * (tierDiscount + couponDiscount);
}

private static decimal GetTierDiscount(string tier) => tier switch
{
    "gold" => 0.2m, "silver" => 0.1m, "bronze" => 0.05m, _ => 0m
};

private static decimal GetCouponDiscount(string code) => code switch
{
    "SAVE10" => 0.1m, "SAVE20" => 0.2m, _ => 0m
};

private decimal CalculateTax(decimal amount) => amount * TaxRate;
private decimal CalculateShipping(List<InvoiceItem> items)
    => items.Any(i => i.Type == "physical") ? ShippingCost : 0m;

private const decimal TaxRate = 0.15m;
private const decimal ShippingCost = 9.99m;
```

> **O que mudou:** Extract Method em cada bloco logico. Cada metodo e testavel isoladamente. Switch expressions em vez de if/else chains. Constantes nomeadas.

---

### 8. Deep Nesting

If/else/for aninhados demais, dificeis de ler e manter.

**RUIM:**

```csharp
public string ProcessOrder(Order order)
{
    if (order != null)
    {
        if (order.Items != null && order.Items.Count > 0)
        {
            if (order.Customer != null)
            {
                if (order.Customer.IsActive)
                {
                    if (order.Total > 0)
                    {
                        foreach (var item in order.Items)
                        {
                            if (item.Stock > 0)
                            {
                                // finalmente a logica real, 8 niveis de indentacao
                                item.Stock--;
                            }
                        }
                        return "Success";
                    }
                }
            }
        }
    }
    return "Failed";
}
```

**BOM (Guard Clauses / Early Return):**

```csharp
public string ProcessOrder(Order order)
{
    if (order is null)
        throw new ArgumentNullException(nameof(order));

    if (order.Items is null or { Count: 0 })
        throw new ValidationException("Order must have at least one item");

    if (order.Customer is null)
        throw new ValidationException("Order must have a customer");

    if (!order.Customer.IsActive)
        throw new BusinessException("Customer account is inactive");

    if (order.Total <= 0)
        throw new ValidationException("Order total must be positive");

    foreach (var item in order.Items.Where(i => i.Stock > 0))
    {
        item.Stock--;
    }

    return "Success";
}
```

> **O que mudou:** Guard clauses com early return eliminam o nesting. Exceptions claras para cada caso invalido. LINQ `Where` em vez de if dentro do foreach.

---

### 9. No Interfaces / No Async

Dependencias concretas e operacoes I/O bloqueantes.

**RUIM:**

```csharp
public class ProductService
{
    private readonly SqlConnection _connection = new("Server=localhost;...");

    public List<Product> GetAll()
    {
        _connection.Open();  // Bloqueia a thread!
        var cmd = new SqlCommand("SELECT * FROM Products", _connection);
        var reader = cmd.ExecuteReader();  // Bloqueia a thread!

        var products = new List<Product>();
        while (reader.Read())
        {
            products.Add(new Product
            {
                Id = reader.GetGuid(0),
                Name = reader.GetString(1),
                Price = reader.GetDecimal(2)
            });
        }
        _connection.Close();
        return products;
    }
}
```

**BOM:**

```csharp
public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct = default);
}

public class SqlProductRepository : IProductRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<SqlProductRepository> _logger;

    public SqlProductRepository(IDbConnectionFactory connectionFactory, ILogger<SqlProductRepository> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct = default)
    {
        _logger.LogDebug("Buscando todos os produtos");

        using var connection = await _connectionFactory.CreateConnectionAsync(ct);
        var products = await connection.QueryAsync<Product>(
            new CommandDefinition("SELECT Id, Name, Price FROM Products", cancellationToken: ct));

        _logger.LogInformation("Retornados {Count} produtos", products.Count());
        return products.ToList().AsReadOnly();
    }
}
```

> **O que mudou:** Interface para abstraction, async/await em toda operacao I/O, CancellationToken, `using` para dispose automatico, Dapper simplificando o mapping, ILogger para logging.

---

### 10. Console.WriteLine em vez de ILogger

Output direto no console sem estrutura, sem niveis, sem filtragem.

**RUIM:**

```csharp
public void Process(Order order)
{
    Console.WriteLine("Processing order " + order.Id);
    Console.WriteLine("Customer: " + order.CustomerName);
    Console.WriteLine("Total: " + order.Total);

    try { /* ... */ }
    catch (Exception ex)
    {
        Console.WriteLine("ERROR: " + ex.Message);  // Perde stack trace!
    }
}
```

**BOM:**

```csharp
public class OrderProcessor
{
    private readonly ILogger<OrderProcessor> _logger;

    public OrderProcessor(ILogger<OrderProcessor> logger) => _logger = logger;

    public void Process(Order order)
    {
        _logger.LogInformation("Processando pedido {OrderId} para {Customer}",
            order.Id, order.CustomerName);

        _logger.LogDebug("Detalhes do pedido: {@Order}", order);

        try { /* ... */ }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao processar pedido {OrderId}", order.Id);
            throw;
        }
    }
}
```

> **O que mudou:** ILogger com structured logging (placeholders, nao concatenacao). LogDebug para payloads, LogError para exceptions (preservando stack trace). Filtravel por nivel e pesquisavel em CloudWatch/Kibana.

---

### 11. Static State

Estado compartilhado global sem controle de concorrencia.

**RUIM:**

```csharp
public class OrderCounter
{
    public static int TotalOrders = 0;       // Acessivel globalmente, sem thread safety
    public static List<string> Errors = new(); // Mesma List compartilhada entre threads

    public static void ProcessOrder()
    {
        TotalOrders++;  // Race condition em ambiente multi-thread!
        if (TotalOrders > 1000)
            Errors.Add("Limit exceeded"); // Nao e thread-safe
    }
}
```

**BOM:**

```csharp
public class OrderMetrics
{
    private int _totalOrders;
    private readonly ConcurrentBag<string> _errors = new();

    public int TotalOrders => _totalOrders;
    public IReadOnlyCollection<string> Errors => _errors;

    public void RecordOrder()
    {
        Interlocked.Increment(ref _totalOrders);
    }

    public void RecordError(string message)
    {
        _errors.Add(message);
    }
}

// Registrar como Singleton no DI (uma instancia compartilhada, mas thread-safe)
builder.Services.AddSingleton<OrderMetrics>();
```

> **O que mudou:** `Interlocked.Increment` para operacoes atomicas, `ConcurrentBag` para colecao thread-safe, instancia gerenciada pelo DI container, propriedades readonly para encapsulamento.

---

## Parte 2: Checklist de Entrega -- Exemplos

---

### 1. Codigo compila e roda sem erros

```bash
# Verificar antes de entregar
dotnet build --no-restore    # compila sem buscar pacotes
dotnet run                   # executa e testa manualmente
```

> Dica: se usar `WebApplicationFactory`, o partial class `Program` precisa existir:
```csharp
// No final do Program.cs
public partial class Program { }
```

---

### 2. Testes unitarios passando

```bash
dotnet test --verbosity normal
```

```csharp
// Minimo esperado: testar os caminhos principais (happy path + edge cases)
[Fact]
public async Task CreateProduct_ValidRequest_ReturnsCreated()
{
    // Arrange
    var request = new CreateProductRequest { Name = "Laptop", Price = 999.99m, Category = "Electronics" };

    // Act
    var response = await _client.PostAsJsonAsync("/products", request);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.Created);
    var product = await response.Content.ReadFromJsonAsync<Product>();
    product!.Name.Should().Be("Laptop");
}

[Fact]
public async Task CreateProduct_EmptyName_ReturnsBadRequest()
{
    var request = new CreateProductRequest { Name = "", Price = 10, Category = "Books" };
    var response = await _client.PostAsJsonAsync("/products", request);
    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
}

[Fact]
public async Task GetProduct_NonExistentId_ReturnsNotFound()
{
    var response = await _client.GetAsync($"/products/{Guid.NewGuid()}");
    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
}
```

---

### 3. Validation em todos os inputs

```csharp
// Cada endpoint que recebe dados deve validar ANTES de processar
app.MapPost("/products", async (CreateProductRequest request, IValidator<CreateProductRequest> validator, ...) =>
{
    var result = await validator.ValidateAsync(request);
    if (!result.IsValid)
        return Results.ValidationProblem(
            result.Errors.GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray()));

    // So chega aqui se for valido
});

// Parametros de rota tambem: usar constraints
app.MapGet("/products/{id:guid}", ...);     // Rejeita se nao for GUID valido
app.MapGet("/orders/{id:int:min(1)}", ...); // Rejeita se nao for int positivo
```

---

### 4. Error handling com mensagens claras

```csharp
// Middleware global de erro
app.Use(async (context, next) =>
{
    try
    {
        await next(context);
    }
    catch (ValidationException ex)
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsJsonAsync(new
        {
            title = "Validation failed",
            status = 400,
            errors = ex.Errors
        });
    }
    catch (KeyNotFoundException ex)
    {
        context.Response.StatusCode = 404;
        await context.Response.WriteAsJsonAsync(new
        {
            title = "Resource not found",
            status = 404,
            detail = ex.Message
        });
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Unhandled exception");
        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(new
        {
            title = "An unexpected error occurred",
            status = 500
            // NUNCA expor ex.Message em producao
        });
    }
});
```

---

### 5. Logging estruturado onde relevante

```csharp
// Entry point de cada operacao importante
_logger.LogInformation("Criando produto {ProductName} na categoria {Category}",
    request.Name, request.Category);

// Resultado de operacoes
_logger.LogInformation("Produto {ProductId} criado com sucesso", product.Id);

// Debug para investigacao
_logger.LogDebug("Request payload: {@Request}", request);

// Erros com exception e contexto
_logger.LogError(ex, "Falha ao criar produto {ProductName}", request.Name);

// NUNCA logar:
// _logger.LogInformation("Password: {Password}", user.Password);     // PII!
// _logger.LogInformation("Token: {Token}", jwt);                      // Secret!
// _logger.LogInformation($"User {user.Name} created");                // String interpolation!
//                                                      Use placeholders ^^^^
```

---

### 6. Async/await em operacoes I/O

```csharp
// Toda operacao que toca banco, rede, arquivo deve ser async
public async Task<Product?> GetByIdAsync(Guid id)
    => await _context.Products.FindAsync(id);

// Chamadas independentes em paralelo
var productsTask = _repo.GetAllAsync(filter);
var categoriesTask = _categoryRepo.GetCategoriesAsync();
await Task.WhenAll(productsTask, categoriesTask);

// Propagar CancellationToken
public async Task<IReadOnlyList<Product>> SearchAsync(string query, CancellationToken ct = default)
{
    return await _context.Products
        .Where(p => p.Name.Contains(query))
        .ToListAsync(ct);
}
```

---

### 7. DI para todas as dependencias

```csharp
// Program.cs -- registrar servicos
builder.Services.AddSingleton<ICache, InMemoryCache>();
builder.Services.AddScoped<IProductRepository, SqlProductRepository>();
builder.Services.AddTransient<IValidator<CreateProductRequest>, CreateProductRequestValidator>();

// Injetar via constructor (classes) ou parameter binding (Minimal APIs)
app.MapGet("/products/{id:guid}", async (Guid id, IProductRepository repo) =>
    await repo.GetByIdAsync(id) is { } product
        ? Results.Ok(product)
        : Results.NotFound());
```

> Nunca fazer `new SqlProductRepository()` dentro de um endpoint ou servico.

---

### 8. Nomes claros e auto-explicativos

```csharp
// RUIM
var d = GetData();
var x = d.Where(i => i.S == 1).ToList();
void Proc(int t, string n) { }

// BOM
var activeProducts = await _repository.GetAllAsync(filter);
var pendingOrders = orders.Where(o => o.Status == OrderStatus.Pending).ToList();
async Task<Product> CreateProductAsync(CreateProductRequest request) { }
```

> Regra: se voce precisa de um comentario para explicar o que uma variavel/metodo faz, renomeie.

---

### 9. Sem codigo comentado ou morto

```csharp
// RUIM: deixar codigo antigo comentado
public decimal Calculate(decimal price)
{
    // var oldDiscount = price * 0.1m;
    // var tax = price * 0.2m;
    // return price - oldDiscount + tax;

    // TODO: remover depois
    // if (price > 100) return price * 0.85m;

    return _pricingEngine.Calculate(price);
}

// BOM: so o codigo que esta em uso
public decimal Calculate(decimal price)
{
    return _pricingEngine.Calculate(price);
}
```

> Se precisar do codigo antigo, ele esta no Git. Nao polua o arquivo com comentarios mortos.

---

### 10. HTTP Status Codes corretos

```csharp
// GET com sucesso → 200 OK
app.MapGet("/products", async (IProductRepository repo) =>
    Results.Ok(await repo.GetAllAsync()));

// GET nao encontrado → 404 Not Found
app.MapGet("/products/{id:guid}", async (Guid id, IProductRepository repo) =>
    await repo.GetByIdAsync(id) is { } product
        ? Results.Ok(product)           // 200
        : Results.NotFound());          // 404

// POST criado → 201 Created (com Location header)
app.MapPost("/products", async (...) =>
    Results.Created($"/products/{product.Id}", product));  // 201

// PUT atualizado → 200 OK (ou 404 se nao existe)
app.MapPut("/products/{id:guid}", async (...) =>
    updated is not null
        ? Results.Ok(updated)           // 200
        : Results.NotFound());          // 404

// DELETE → 204 No Content (ou 404 se nao existe)
app.MapDelete("/products/{id:guid}", async (Guid id, IProductRepository repo) =>
    await repo.DeleteAsync(id)
        ? Results.NoContent()           // 204
        : Results.NotFound());          // 404

// Validation error → 400 Bad Request
Results.ValidationProblem(errors);       // 400

// Erro interno → 500 (via middleware global)
Results.Problem("Internal error", statusCode: 500);
```

---

> **Resumo:** Cada red flag tem uma solucao clara. O segredo e aplicar **SOLID + DI + async + validation + structured logging + testes**. Se voce lembrar desses 6 pilares, vai cobrir 95% do que o entrevistador espera.
