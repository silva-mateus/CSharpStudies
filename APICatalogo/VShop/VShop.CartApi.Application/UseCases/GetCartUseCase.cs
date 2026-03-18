using AutoMapper;
using VShop.CartApi.Application.DTOs;
using VShop.CartApi.Domain.Interfaces;

namespace VShop.CartApi.Application.UseCases;

public class GetCartUseCase
{
    private readonly ICartRepository _repository;
    private readonly IMapper _mapper;

    public GetCartUseCase(ICartRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CartDTO> ExecuteAsync(string userId)
    {
        var cart = await _repository.GetCartByUserIdAsync(userId);
        return _mapper.Map<CartDTO>(cart);
    }
}
