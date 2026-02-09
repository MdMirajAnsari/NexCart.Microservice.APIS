using System.Linq.Expressions;
using NexCart.Orders.DTO;
using NexCart.Orders.Entities;

namespace NexCart.Orders.ServiceContracts
{
    public interface IOrdersService
    {
        Task<IEnumerable<OrderResponse?>> GetOrders();
        Task<OrderResponse?> GetOrderByCondition(Expression<Func<Order, bool>> predicate);
        Task<OrderResponse?> AddOrder(OrderAddRequest request);
    }
}
