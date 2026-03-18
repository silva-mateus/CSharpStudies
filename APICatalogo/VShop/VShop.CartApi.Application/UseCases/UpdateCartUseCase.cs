using AutoMapper;
using VShop.CartApi.Application.DTOs;
using VShop.CartApi.Domain.Entities;
using VShop.CartApi.Domain.Interfaces;

namespace VShop.CartApi.Application.UseCases;

public class UpdateCartUseCase
{
    private readonly ICartRepository _repository;
    private readonly IMapper _mapper;

    public UpdateCartUseCase(ICartRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CartDTO> ExecuteAsync(CartDTO cartDto)
    {
        var cart = _mapper.Map<Cart>(cartDto);
        var updated = await _repository.UpdateCartAsync(cart);
        return _mapper.Map<CartDTO>(updated);
    }
}
