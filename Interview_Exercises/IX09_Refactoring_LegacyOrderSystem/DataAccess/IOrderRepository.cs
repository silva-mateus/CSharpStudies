using IX09_Refactoring_LegacyOrderSystem.Models;

namespace IX09_Refactoring_LegacyOrderSystem.DataAccess;

public interface IOrderRepository
{
    void Save(string orderId, ProcessedOrderResult result);
    ProcessedOrderResult? Get(string orderId);
}
