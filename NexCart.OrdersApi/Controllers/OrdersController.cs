using Microsoft.AspNetCore.Mvc;
using NexCart.Orders.ServiceContracts;
using NexCart.Orders.DTO;
using System.Threading.Tasks;
using System;

namespace NexCart.OrdersApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _ordersService.GetOrders();
            return Ok(orders);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var order = await _ordersService.GetOrderByCondition(k => k.OrderID == id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderAddRequest request)
        {
            var result = await _ordersService.AddOrder(request);
            if (result == null) return BadRequest();
            return CreatedAtAction(nameof(GetById), new { id = result.OrderID }, result);
        }
    }
}
