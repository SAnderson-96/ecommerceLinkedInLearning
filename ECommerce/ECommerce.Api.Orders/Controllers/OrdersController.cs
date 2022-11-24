using ECommerce.Api.Orders.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Orders.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private IOrdersProvider _provider;

        public OrdersController(IOrdersProvider provider)
        {
            _provider = provider;
        }
        [HttpGet]
        public async Task<IActionResult> GetOrdersAsync()
        {
            var result = await _provider.GetOrdersAsync();
            if (result.IsSuccess)
            {
                return Ok(result.Orders);
            }
            return NotFound();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrdersFromCustomerIdAsync(int id)
        {
            var result = await _provider.GetOrdersFromCustomerIdAsync(id);

            if (result.IsSuccess)
            {
                return Ok(result.Orders);
            }
            return NotFound();
        }
    }
}
