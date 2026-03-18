using VShop.CartApi.Application.DTOs;
using VShop.CartApi.Application.UseCases;

namespace VShop.CartApi.Api.Endpoints;

public static class CartEndpoints
{
    public static void MapCartEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/cart");

        group.MapGet("/getcart/{userId}", async (string userId, GetCartUseCase useCase) =>
        {
            var cart = await useCase.ExecuteAsync(userId);
            return Results.Ok(cart);
        });

        group.MapPost("/addcart", async (CartDTO cartDto, UpdateCartUseCase useCase) =>
        {
            var cart = await useCase.ExecuteAsync(cartDto);
            return Results.Ok(cart);
        });

        group.MapPut("/updatecart", async (CartDTO cartDto, UpdateCartUseCase useCase) =>
        {
            var cart = await useCase.ExecuteAsync(cartDto);
            return Results.Ok(cart);
        });

        group.MapDelete("/deletecart/{id:int}", async (int id, DeleteCartItemUseCase useCase) =>
        {
            var result = await useCase.ExecuteAsync(id);
            return result ? Results.Ok(result) : Results.NotFound();
        });

        group.MapPut("/applycoupon", async (CartDTO cartDto, ApplyCouponUseCase useCase) =>
        {
            var result = await useCase.ExecuteAsync(
                cartDto.CartHeader.UserId, cartDto.CartHeader.CouponCode);
            return result ? Results.Ok(result) : Results.NotFound();
        });

        group.MapDelete("/deletecoupon/{userId}", async (string userId, RemoveCouponUseCase useCase) =>
        {
            var result = await useCase.ExecuteAsync(userId);
            return result ? Results.Ok(result) : Results.NotFound();
        });

        group.MapPost("/checkout", async (CheckoutHeaderDTO checkoutHeaderDto, CheckoutUseCase useCase) =>
        {
            var result = await useCase.ExecuteAsync(checkoutHeaderDto);
            return result is null ? Results.NotFound("Cart not found") : Results.Ok(result);
        });
    }
}
