using AutoMapper;
using VShop.DiscountApi.Application.DTOs;
using VShop.DiscountApi.Domain.Entities;
using VShop.DiscountApi.Domain.Interfaces;

namespace VShop.DiscountApi.Application.UseCases.Coupons;

public class CreateCouponUseCase
{
    private readonly ICouponRepository _repository;
    private readonly IMapper _mapper;

    public CreateCouponUseCase(ICouponRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CouponDTO> ExecuteAsync(CouponDTO dto)
    {
        var coupon = _mapper.Map<Coupon>(dto);
        coupon.Validate();
        await _repository.AddAsync(coupon);
        return _mapper.Map<CouponDTO>(coupon);
    }
}
