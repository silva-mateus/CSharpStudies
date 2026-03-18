namespace VShop.CartApi.Application.DTOs;

public class CartDTO
{
    public CartHeaderDTO CartHeader { get; set; } = new CartHeaderDTO();
    public IEnumerable<CartItemDTO> CartItems { get; set; } = Enumerable.Empty<CartItemDTO>();
}
