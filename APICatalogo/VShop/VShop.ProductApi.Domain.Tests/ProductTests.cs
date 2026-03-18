using VShop.ProductApi.Domain.Entities;
using VShop.ProductApi.Domain.Exceptions;

namespace VShop.ProductApi.Domain.Tests;

public class ProductTests
{
    [Fact]
    public void Validate_WithValidData_DoesNotThrow()
    {
        var product = new Product
        {
            Name = "Caderno",
            Price = 10,
            Description = "Caderno espiral",
            Stock = 5,
            CategoryId = 1
        };
        product.Validate();
    }

    [Fact]
    public void Validate_WithEmptyName_ThrowsDomainException()
    {
        var product = new Product { Name = "", Price = 10, Description = "Valid desc", Stock = 1 };
        Assert.Throws<DomainException>(() => product.Validate());
    }

    [Fact]
    public void Validate_WithShortName_ThrowsDomainException()
    {
        var product = new Product { Name = "Ab", Price = 10, Description = "Valid desc", Stock = 1 };
        Assert.Throws<DomainException>(() => product.Validate());
    }

    [Fact]
    public void Validate_WithNegativePrice_ThrowsDomainException()
    {
        var product = new Product { Name = "Caderno", Price = -1, Description = "Valid desc", Stock = 1 };
        Assert.Throws<DomainException>(() => product.Validate());
    }

    [Fact]
    public void Validate_WithZeroPrice_ThrowsDomainException()
    {
        var product = new Product { Name = "Caderno", Price = 0, Description = "Valid desc", Stock = 1 };
        Assert.Throws<DomainException>(() => product.Validate());
    }

    [Fact]
    public void Validate_WithEmptyDescription_ThrowsDomainException()
    {
        var product = new Product { Name = "Caderno", Price = 10, Description = "", Stock = 1 };
        Assert.Throws<DomainException>(() => product.Validate());
    }

    [Fact]
    public void Validate_WithNegativeStock_ThrowsDomainException()
    {
        var product = new Product { Name = "Caderno", Price = 10, Description = "Valid desc", Stock = -1 };
        Assert.Throws<DomainException>(() => product.Validate());
    }
}
