using AutoMapper;
using NexCart.Orders.DTO;
using NexCart.Orders.Entities;
using NexCart.Orders.RepositoryContracts;
using NexCart.Orders.ServiceContracts;
using System.Linq.Expressions;

namespace NexCart.Orders.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IMapper _mapper;

        public OrdersService(IOrdersRepository ordersRepository, IMapper mapper)
        {
            _ordersRepository = ordersRepository;
            _mapper = mapper;
        }

        public async Task<OrderResponse?> AddOrder(OrderAddRequest request)
        {
            if (request == null) return null;
            var order = _mapper.Map<Order>(request);
            var added = await _ordersRepository.AddOrder(order);
            return added == null ? null : _mapper.Map<OrderResponse>(added);
        }

        public async Task<OrderResponse?> GetOrderByCondition(Expression<Func<Order, bool>> predicate)
        {
            var order = await _ordersRepository.GetOrderByCondition(predicate);
            return order == null ? null : _mapper.Map<OrderResponse>(order);
        }

        public async Task<IEnumerable<OrderResponse?>> GetOrders()
        {
            var orders = await _ordersRepository.GetOrders();
            return _mapper.Map<IEnumerable<OrderResponse?>>(orders);
        }
    }
}
