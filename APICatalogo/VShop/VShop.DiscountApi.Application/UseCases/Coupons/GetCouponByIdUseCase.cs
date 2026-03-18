using AutoMapper;
using VShop.DiscountApi.Application.DTOs;
using VShop.DiscountApi.Domain.Interfaces;

namespace VShop.DiscountApi.Application.UseCases.Coupons;

public class GetCouponByIdUseCase
{
    private readonly ICouponRepository _repository;
    private readonly IMapper _mapper;

    public GetCouponByIdUseCase(ICouponRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CouponDTO?> ExecuteAsync(int id)
    {
        var coupon = await _repository.GetByIdAsync(id);
        return coupon is null ? null : _mapper.Map<CouponDTO>(coupon);
    }
}
