using VShop.DiscountApi.Application.DTOs;
using VShop.DiscountApi.Application.UseCases.Coupons;

namespace VShop.DiscountApi.Api.Endpoints;

public static class CouponEndpoints
{
    public static void MapCouponEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/coupon");

        group.MapGet("/", async (GetAllCouponsUseCase useCase) =>
            Results.Ok(await useCase.ExecuteAsync()));

        group.MapGet("/{id:int}", async (int id, GetCouponByIdUseCase useCase) =>
        {
            var coupon = await useCase.ExecuteAsync(id);
            return coupon is null ? Results.NotFound("Coupon not found") : Results.Ok(coupon);
        }).WithName("GetCoupon");

        group.MapGet("/{couponCode}", async (string couponCode, GetCouponByCodeUseCase useCase) =>
        {
            var coupon = await useCase.ExecuteAsync(couponCode);
            return coupon is null ? Results.NotFound("Coupon not found") : Results.Ok(coupon);
        });

        group.MapPost("/", async (CouponDTO dto, CreateCouponUseCase useCase) =>
        {
            var created = await useCase.ExecuteAsync(dto);
            return Results.CreatedAtRoute("GetCoupon", new { id = created.CouponId }, created);
        }).RequireAuthorization(p => p.RequireRole("Admin"));

        group.MapPut("/", async (CouponDTO dto, UpdateCouponUseCase useCase) =>
        {
            var updated = await useCase.ExecuteAsync(dto);
            return Results.Ok(updated);
        }).RequireAuthorization(p => p.RequireRole("Admin"));

        group.MapDelete("/{id:int}", async (int id, DeleteCouponUseCase useCase) =>
        {
            var result = await useCase.ExecuteAsync(id);
            return result ? Results.Ok(result) : Results.NotFound("Coupon not found");
        }).RequireAuthorization(p => p.RequireRole("Admin"));
    }
}
