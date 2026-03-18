using AutoMapper;
using VShop.DiscountApi.Application.DTOs;
using VShop.DiscountApi.Domain.Interfaces;

namespace VShop.DiscountApi.Application.UseCases.Coupons;

public class GetCouponByCodeUseCase
{
    private readonly ICouponRepository _repository;
    private readonly IMapper _mapper;

    public GetCouponByCodeUseCase(ICouponRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CouponDTO?> ExecuteAsync(string couponCode)
    {
        var coupon = await _repository.GetByCodeAsync(couponCode);
        return coupon is null ? null : _mapper.Map<CouponDTO>(coupon);
    }
}
