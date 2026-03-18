using VShop.ProductApi.Domain.Entities;
using VShop.ProductApi.Domain.Exceptions;

namespace VShop.ProductApi.Domain.Tests;

public class CategoryTests
{
    [Fact]
    public void Validate_WithValidData_DoesNotThrow()
    {
        var category = new Category { Name = "Material Escolar" };
        category.Validate();
    }

    [Fact]
    public void Validate_WithEmptyName_ThrowsDomainException()
    {
        var category = new Category { Name = "" };
        Assert.Throws<DomainException>(() => category.Validate());
    }

    [Fact]
    public void Validate_WithShortName_ThrowsDomainException()
    {
        var category = new Category { Name = "Ab" };
        Assert.Throws<DomainException>(() => category.Validate());
    }

    [Fact]
    public void Validate_WithLongName_ThrowsDomainException()
    {
        var category = new Category { Name = new string('A', 101) };
        Assert.Throws<DomainException>(() => category.Validate());
    }
}
