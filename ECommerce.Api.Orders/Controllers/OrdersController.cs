﻿using ECommerce.Api.Orders.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.Api.Orders.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersProvider _ordersProvider;

        public OrdersController(IOrdersProvider ordersProvider)
        {
            this._ordersProvider = ordersProvider;
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetOrdersAsync(int customerId)
        {
            var result = await _ordersProvider.GetOrdersAsync(customerId);

            if (result.IsSuccess)
            {
                return Ok(result.Orders);
            }

            return NotFound();
        }
    }
}