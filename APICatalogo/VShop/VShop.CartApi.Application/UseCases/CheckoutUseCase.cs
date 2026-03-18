using AutoMapper;
using VShop.CartApi.Application.DTOs;
using VShop.CartApi.Application.Interfaces;
using VShop.CartApi.Domain.Interfaces;

namespace VShop.CartApi.Application.UseCases;

public class CheckoutUseCase
{
    private readonly ICartRepository _cartRepository;
    private readonly IMessageProducer _messageProducer;
    private readonly IMapper _mapper;

    public CheckoutUseCase(
        ICartRepository cartRepository,
        IMessageProducer messageProducer,
        IMapper mapper)
    {
        _cartRepository = cartRepository;
        _messageProducer = messageProducer;
        _mapper = mapper;
    }

    public async Task<CheckoutHeaderDTO?> ExecuteAsync(CheckoutHeaderDTO checkoutHeader)
    {
        var cart = await _cartRepository.GetCartByUserIdAsync(checkoutHeader.UserId);
        if (cart.CartHeader.Id == 0)
            return null;

        var cartDto = _mapper.Map<CartDTO>(cart);
        checkoutHeader.CartItems = cartDto.CartItems;
        checkoutHeader.DateTime = DateTime.UtcNow;

        await _messageProducer.PublishCheckoutAsync(checkoutHeader);
        await _cartRepository.CleanCartAsync(checkoutHeader.UserId);

        return checkoutHeader;
    }
}
