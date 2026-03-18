using Microsoft.Extensions.Logging;
using VShop.CartApi.Application.DTOs;
using VShop.CartApi.Application.Interfaces;

namespace VShop.CartApi.Infrastructure.Messaging;

public class NullMessageProducer : IMessageProducer
{
    private readonly ILogger<NullMessageProducer> _logger;

    public NullMessageProducer(ILogger<NullMessageProducer> logger)
    {
        _logger = logger;
    }

    public Task PublishCheckoutAsync(CheckoutHeaderDTO checkoutHeader)
    {
        _logger.LogWarning("Message producer not configured. Checkout message for user {UserId} discarded.",
            checkoutHeader.UserId);
        return Task.CompletedTask;
    }
}
