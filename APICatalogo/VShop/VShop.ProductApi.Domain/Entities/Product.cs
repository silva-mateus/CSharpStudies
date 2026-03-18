using VShop.ProductApi.Domain.Exceptions;

namespace VShop.ProductApi.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public long Stock { get; set; }
    public string? ImageURL { get; set; }

    public Category? Category { get; set; }
    public int CategoryId { get; set; }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new DomainException("Product name is required.");

        if (Name.Length < 3)
            throw new DomainException("Product name must be at least 3 characters.");

        if (Name.Length > 100)
            throw new DomainException("Product name must be at most 100 characters.");

        if (Price <= 0)
            throw new DomainException("Price must be greater than zero.");

        if (string.IsNullOrWhiteSpace(Description))
            throw new DomainException("Description is required.");

        if (Description.Length < 5)
            throw new DomainException("Description must be at least 5 characters.");

        if (Stock < 0)
            throw new DomainException("Stock cannot be negative.");
    }
}
