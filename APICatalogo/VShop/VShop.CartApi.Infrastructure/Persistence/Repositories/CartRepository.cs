using Microsoft.EntityFrameworkCore;
using VShop.CartApi.Domain.Entities;
using VShop.CartApi.Domain.Interfaces;

namespace VShop.CartApi.Infrastructure.Persistence.Repositories;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _context;

    public CartRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Cart> GetCartByUserIdAsync(string userId)
    {
        Cart cart = new()
        {
            CartHeader = await _context.CartHeaders
                .FirstOrDefaultAsync(c => c.UserId == userId) ?? new CartHeader()
        };

        cart.CartItems = await _context.CartItems
            .Where(c => c.CartHeaderId == cart.CartHeader.Id)
            .Include(c => c.Product)
            .ToListAsync();

        return cart;
    }

    public async Task<Cart> UpdateCartAsync(Cart cart)
    {
        var productInDb = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == cart.CartItems.First().ProductId);

        if (productInDb is null)
        {
            _context.Products.Add(cart.CartItems.First().Product);
            await _context.SaveChangesAsync();
        }

        var cartHeaderInDb = await _context.CartHeaders
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.UserId == cart.CartHeader.UserId);

        if (cartHeaderInDb is null)
        {
            _context.CartHeaders.Add(cart.CartHeader);
            await _context.SaveChangesAsync();

            cart.CartItems.First().CartHeaderId = cart.CartHeader.Id;
            cart.CartItems.First().Product = null!;
            _context.CartItems.Add(cart.CartItems.First());
            await _context.SaveChangesAsync();
        }
        else
        {
            var cartItemInDb = await _context.CartItems
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.ProductId == cart.CartItems.First().ProductId
                                       && c.CartHeaderId == cartHeaderInDb.Id);

            if (cartItemInDb is null)
            {
                cart.CartItems.First().CartHeaderId = cartHeaderInDb.Id;
                cart.CartItems.First().Product = null!;
                _context.CartItems.Add(cart.CartItems.First());
                await _context.SaveChangesAsync();
            }
            else
            {
                cart.CartItems.First().Product = null!;
                cart.CartItems.First().Id = cartItemInDb.Id;
                cart.CartItems.First().CartHeaderId = cartHeaderInDb.Id;
                cart.CartItems.First().Quantity += cartItemInDb.Quantity;
                _context.CartItems.Update(cart.CartItems.First());
                await _context.SaveChangesAsync();
            }
        }

        return cart;
    }

    public async Task<bool> DeleteItemCartAsync(int cartItemId)
    {
        try
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.Id == cartItemId);

            if (cartItem is null) return false;

            int totalItems = await _context.CartItems
                .Where(c => c.CartHeaderId == cartItem.CartHeaderId)
                .CountAsync();

            _context.CartItems.Remove(cartItem);

            if (totalItems == 1)
            {
                var cartHeader = await _context.CartHeaders
                    .FirstOrDefaultAsync(c => c.Id == cartItem.CartHeaderId);

                if (cartHeader is not null)
                    _context.CartHeaders.Remove(cartHeader);
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> ApplyCouponAsync(string userId, string couponCode)
    {
        var cartHeader = await _context.CartHeaders
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cartHeader is null) return false;

        cartHeader.CouponCode = couponCode;
        _context.CartHeaders.Update(cartHeader);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteCouponAsync(string userId)
    {
        var cartHeader = await _context.CartHeaders
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cartHeader is null) return false;

        cartHeader.CouponCode = string.Empty;
        _context.CartHeaders.Update(cartHeader);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CleanCartAsync(string userId)
    {
        var cartHeader = await _context.CartHeaders
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cartHeader is null) return false;

        var cartItems = await _context.CartItems
            .Where(c => c.CartHeaderId == cartHeader.Id)
            .ToListAsync();

        _context.CartItems.RemoveRange(cartItems);
        _context.CartHeaders.Remove(cartHeader);
        await _context.SaveChangesAsync();
        return true;
    }
}
