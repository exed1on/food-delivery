using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using food_delivery.Domain;
using food_delivery.Service;
using food_delivery.Dto;
using Microsoft.AspNetCore.Authorization;

namespace food_delivery.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost("register/")]
        public ActionResult<string> RegisterNewCustomer([FromBody] RegisterDto newCustomer)
        {
            var customer = _customerService.AddCustomer(newCustomer);

            return customer;
        }

        [Authorize]
        [HttpPut("update/")]
        public ActionResult<Customer> UpdateExistingCustomer([FromBody] RegisterDto newCustomer)
        {
            var customer = _customerService.UpdateCustomer(newCustomer);

            return customer;
        }

        [Authorize]
        [HttpDelete("delete/")]
        public ActionResult<Customer> DeleteCustomer([FromBody] Credentials creds)
        {
            var customer = _customerService.DeleteCustomer(creds);

            return customer;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpHead("/api/Customer/{userName}")]
        public IActionResult CheckCustomer(string userName)
        {
            var customer = _customerService.GetCustomerByUsername(userName);
            if (customer != null)
            {               
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpPost("deposit/")]
        public ActionResult<Customer> RegisterNewCustomer(string userName, double amount)
        {
            if (amount <= 0)
            {
                return BadRequest("Invalid deposit request.");
            }

            var customer = _customerService.DepositMoney(userName, amount);

            return customer;
        }
    }
}