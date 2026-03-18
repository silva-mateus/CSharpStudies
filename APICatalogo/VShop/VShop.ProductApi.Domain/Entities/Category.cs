using VShop.ProductApi.Domain.Exceptions;

namespace VShop.ProductApi.Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public ICollection<Product>? Products { get; set; }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new DomainException("Category name is required.");

        if (Name.Length < 3)
            throw new DomainException("Category name must be at least 3 characters.");

        if (Name.Length > 100)
            throw new DomainException("Category name must be at most 100 characters.");
    }
}
