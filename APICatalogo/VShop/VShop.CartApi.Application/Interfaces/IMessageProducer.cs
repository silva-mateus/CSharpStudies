using VShop.CartApi.Application.DTOs;

namespace VShop.CartApi.Application.Interfaces;

public interface IMessageProducer
{
    Task PublishCheckoutAsync(CheckoutHeaderDTO checkoutHeader);
}
