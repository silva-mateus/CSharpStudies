using AutoMapper;
using VShop.DiscountApi.Application.DTOs;
using VShop.DiscountApi.Domain.Entities;
using VShop.DiscountApi.Domain.Interfaces;

namespace VShop.DiscountApi.Application.UseCases.Coupons;

public class UpdateCouponUseCase
{
    private readonly ICouponRepository _repository;
    private readonly IMapper _mapper;

    public UpdateCouponUseCase(ICouponRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CouponDTO> ExecuteAsync(CouponDTO dto)
    {
        var coupon = _mapper.Map<Coupon>(dto);
        coupon.Validate();
        await _repository.UpdateAsync(coupon);
        return _mapper.Map<CouponDTO>(coupon);
    }
}
