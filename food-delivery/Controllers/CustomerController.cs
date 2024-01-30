using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using food_delivery.Domain;
using food_delivery.Service;
using food_delivery.Dto;
using Microsoft.AspNetCore.Authorization;
using Azure.Identity;

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
        [HttpGet("balance/{userName}")]
        public ActionResult<decimal> GetCustomerBalance(string userName)
        {
            var customer = _customerService.GetCustomerByUsername(userName);

            return customer.Balance;
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
            if(creds == null)
            {
                throw new ArgumentNullException("You need to provide proper credentials");
            }
            if (_customerService.GetCustomerByUsername(creds.UserName).Password != creds.Password)
            {
                throw new AuthenticationException("Wrong password");
            }
            var customer = _customerService.DeleteCustomer(creds);
            return customer;
        }

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
        public ActionResult<CustomerDto> DepostitMoney(string userName, double amount)
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