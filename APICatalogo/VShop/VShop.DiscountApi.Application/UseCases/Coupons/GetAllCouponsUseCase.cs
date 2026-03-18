using AutoMapper;
using VShop.DiscountApi.Application.DTOs;
using VShop.DiscountApi.Domain.Interfaces;

namespace VShop.DiscountApi.Application.UseCases.Coupons;

public class GetAllCouponsUseCase
{
    private readonly ICouponRepository _repository;
    private readonly IMapper _mapper;

    public GetAllCouponsUseCase(ICouponRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CouponDTO>> ExecuteAsync()
    {
        var coupons = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<CouponDTO>>(coupons);
    }
}
