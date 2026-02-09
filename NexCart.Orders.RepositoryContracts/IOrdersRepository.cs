using System.Linq.Expressions;
using NexCart.Orders.Entities;

namespace NexCart.Orders.RepositoryContracts
{
    public interface IOrdersRepository
    {
        /// <summary>
        /// Retrieves all Orders asynchronously
        /// </summary>
        /// <returns>Returns all orders</returns>
        Task<IEnumerable<Order>> GetOrders();

        /// <summary>
        /// Retrieves all Orders based on the specified condition asynchronously
        /// </summary>
        /// <param name="predicate">The condition to filter orders</param>
        /// <returns>Returning a collection of matching orders</returns>
        Task<IEnumerable<Order?>> GetOrdersByCondition(Expression<Func<Order, bool>> predicate);

        /// <summary>
        /// Retrieves a single order based on the specified condition asynchronously
        /// </summary>
        /// <param name="predicate">The condition to filter Orders</param>
        /// <returns>Returning matching order</returns>
        Task<Order?> GetOrderByCondition(Expression<Func<Order, bool>> predicate);


        /// <summary>
        /// Adds a new Order asynchronously
        /// </summary>
        /// <param name="order">The order to be added</param>
        /// <returns>Returns the added Order object or null if unsuccessful</returns>
        Task<Order?> AddOrder(Order order);


        /// <summary>
        /// Updates an existing order asynchronously
        /// </summary>
        /// <param name="order">The order to be updated</param>
        /// <returns>Returns the updated order object; or null if not found</returns>
        Task<Order?> UpdateOrder(Order order);


        /// <summary>
        /// Deletes the order asynchronously
        /// </summary>
        /// <param name="orderID">The Order ID based on which we need to delete the order</param>
        /// <returns>Returns true if the deletion is successful, false otherwise</returns>
        Task<bool> DeleteOrder(Guid orderID);
    }
}
