using VShop.Web.Models;

namespace VShop.Web.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductViewModel>?> GetProductsAsync(string token);
    Task<ProductViewModel?> GetProductByIdAsync(int id, string token);
    Task<ProductViewModel?> CreateProductAsync(ProductViewModel product, string token);
    Task<ProductViewModel?> UpdateProductAsync(ProductViewModel product, string token);
    Task<bool> DeleteProductAsync(int id, string token);
}
