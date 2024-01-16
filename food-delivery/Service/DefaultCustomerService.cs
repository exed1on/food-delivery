using System;
using System.Security.Authentication;
using food_delivery.Domain;
using food_delivery.Service;
using Microsoft.EntityFrameworkCore;
using food_delivery.Dto;

namespace food_delivery.Service
{
    public class DefaultCustomerService : ICustomerService
    {
        private readonly List<Customer> customerList;
        private readonly AppDbContext _dbContext;

        public DefaultCustomerService()
        {
        }
        public DefaultCustomerService(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            customerList = new List<Customer>(_dbContext.Customers);
        }
        public Customer AddCustomer(CustomerDto customerDto)
        {
            if (customerDto == null)
            {
                throw new ArgumentNullException(nameof(customerDto));
            }

            if (_dbContext.Customers.Any(c => c.UserName == customerDto.UserName))
            {
                throw new ArgumentException("User with this username already exists");
            }
            Cart newCart = CreateNewCart();

            Customer customer = new Customer(
                userName: customerDto.UserName,
                password: customerDto.Password,
                name: customerDto.Name,
                balance: 0,
                cart: newCart
            );
            var addedCustomer = _dbContext.Customers.Add(customer).Entity;
            _dbContext.SaveChanges();

            return addedCustomer;
        }

        public Customer UpdateCustomer(CustomerDto customerDto)
        {
            var existingCustomer = _dbContext.Customers.SingleOrDefault(c => c.UserName == customerDto.UserName);

            if (existingCustomer == null)
            {
                throw new ArgumentException("User with this username does not exist");
            }

            existingCustomer.Name = customerDto.Name;
            existingCustomer.Password = customerDto.Password;

            _dbContext.SaveChanges();

            return existingCustomer;
        }
        public Customer DeleteCustomer(Credentials creds)
        {
            var existingCustomer = _dbContext.Customers.SingleOrDefault(c => c.UserName == creds.UserName);

            if (existingCustomer == null)
            {
                throw new ArgumentException("User with this username does not exist");
            }

            _dbContext.Customers.Remove(existingCustomer);

            _dbContext.SaveChanges();

            return existingCustomer;
        }
        public Customer GetCustomerByUsername(string username)
        {
            var customer = _dbContext.Customers.SingleOrDefault(c => c.UserName == username);

            if (customer == null)
            {
                throw new ArgumentException("User with this username does not exist");
            }

            return customer;
        }

        public bool CheckCustomerByUsername(string username)
        {
            var customer = _dbContext.Customers.SingleOrDefault(c => c.UserName == username);

            if (customer == null)
            {
                throw new ArgumentException("User with this username does not exist");          
            }

            return true;
        }

        private Cart CreateNewCart()
        {
            return new Cart();
        }

        public Customer DepositMoney(string userName, double amount)
        {
            var customer = GetCustomerByUsername(userName);

            if (customer == null)
            {
                throw new ArgumentException("User with this username does not exist", nameof(userName));
            }
            decimal depositAmount = (decimal)amount;

            customer.Balance += depositAmount;

            _dbContext.SaveChanges();

            return customer;
        }

        public Customer Authenticate(Credentials credentials)
        {
            var existingCustomer = customerList.FirstOrDefault(customer =>
                credentials.UserName == customer.UserName && credentials.Password == customer.Password);

            if (existingCustomer != null)
                return existingCustomer;

            throw new AuthenticationException("Incorrect credentials");
        }

       
    }
}