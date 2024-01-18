using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using food_delivery.Domain;
using food_delivery.Service;
using food_delivery.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace food_delivery.Controllers
{

   [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;

        public OrderController(IOrderService orderService, ICustomerService customerService)
        {
            _orderService = orderService;
            _customerService = customerService;
        }

        [Authorize]
        [HttpPost("createOrder")]
        public ActionResult<OrderDto> CreateOrder(string customerUsername)
        {
            var customer = _customerService.GetCustomerByUsername(customerUsername);

            if (customer == null)
            {
                return NotFound("Customer not found");
            }

            try
            {
                OrderDto order = _orderService.CreateOrder(customer);
                return Ok(order);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("getOrdersByCustomer")]
        public ActionResult<IEnumerable<OrderDto>> GetOrdersByCustomer(string customerUsername)
        {
            var customer = _customerService.GetCustomerByUsername(customerUsername);

            if (customer == null)
            {
                return NotFound("Customer not found");
            }

            try
            {
                IEnumerable<OrderDto> orders = _orderService.GetOrdersByCustomer(customer);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving orders");
            }
        }
    }
}