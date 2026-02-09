using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using NexCart.Orders.Entities;
using NexCart.Orders.RepositoryContracts;

namespace NexCart.Orders.Repositories
{
    public class OrdersDbContext : DbContext
    {
        public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
    }

    public class OrdersRepository : IOrdersRepository
    {
        private readonly OrdersDbContext _context;

        public OrdersRepository(OrdersDbContext context)
        {
            _context = context;
        }

        public async Task<Order?> AddOrder(Order order)
        {
            order.OrderID = Guid.NewGuid();
            foreach (OrderItem orderItem in order.OrderItems)
            {
                orderItem._id = Guid.NewGuid();
            }
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<bool> DeleteOrder(Guid orderID)
        {
            Order? existingOrder = await _context.Orders.FirstOrDefaultAsync(o => o.OrderID == orderID);
            if (existingOrder == null)
            {
                return false;
            }
            _context.Orders.Remove(existingOrder);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Order?> GetOrderByCondition(Expression<Func<Order, bool>> predicate)
        {
            return await _context.Orders.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<Order?>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<IEnumerable<Order?>> GetOrdersByCondition(Expression<Func<Order, bool>> predicate)
        {
            return await _context.Orders.Where(predicate).ToListAsync();
        }

        public async Task<Order?> UpdateOrder(Order order)
        {
            Order? existingOrder = await _context.Orders.FirstOrDefaultAsync(o => o.OrderID == order.OrderID);
            if (existingOrder == null)
            {
                return null;
            }
            existingOrder.OrderDate = order.OrderDate;
            existingOrder.TotalBill = order.TotalBill;
            existingOrder.UserID = order.UserID;
            existingOrder.OrderItems = order.OrderItems;
            await _context.SaveChangesAsync();
            return existingOrder;
        }
    }
}
