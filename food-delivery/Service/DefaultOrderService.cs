using System;
using System.Security.Authentication;
using food_delivery.Domain;
using food_delivery.Dto;
using food_delivery.Service;
using Microsoft.EntityFrameworkCore;

namespace food_delivery.Service
{
    public class DefaultOrderService : IOrderService
    {
        private readonly List<Food> foodList;
        private readonly List<Customer> customerList;
        private readonly AppDbContext _dbContext;
        private readonly IFoodDeliveryService _foodDeliveryService;
        private readonly ICartService _cartService;



        public DefaultOrderService()
        {

        }
        public DefaultOrderService(AppDbContext dbContext, IFoodDeliveryService foodDeliveryService, ICartService cartService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _foodDeliveryService = foodDeliveryService;
            _cartService = cartService;
        }

        public OrderDto CreateOrder(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            var cart = customer.Cart;

            if (cart == null || cart.OrderItems == null || cart.OrderItems.Count == 0)
                throw new InvalidOperationException("Cart cannot be empty");

            var order = new Order(customer);

            if(customer.Balance < order.Price)
            {
                throw new LowBalanceException("NotEnoughBalance", "You do not have enough money on Balance to create this order");
            }
            customer.Balance -= order.Price;
            customer.Cart = Cart.GetEmptyCart();

            order.TimestampCreated = DateTime.UtcNow;

            _dbContext.Orders.Add(order);

            cart.OrderItems.Select(item => new OrderItem
            {
                Food = _foodDeliveryService.GetFoodByName(_foodDeliveryService.GetFoodNameById(item.FoodId)),
                Pieces = item.Pieces,
                Price = item.Price
            }).ToList();

            _dbContext.Carts.Update(cart);
            _dbContext.Customers.Update(customer);
            _dbContext.SaveChanges();

            var orderDto = new OrderDto
            {
                OrderId = order.OrderId,
                Customer = new CustomerDto
                {
                    UserName = order.Customer.Name,
                    Balance = customer.Balance,
                },
                Cart = new CartDto
                {
                    CartId = order.Cart.CartId,
                    Price = order.Cart.Price,
                    OrderItems = order.Cart.OrderItems.Select(item => new OrderItemDto
                    {
                        OrderItemId = item.OrderItemId,
                        FoodName = _foodDeliveryService.GetFoodNameById(item.FoodId),
                        Pieces = item.Pieces,
                        Price = item.Price
                    }).ToList()
                },
                Price = order.Cart.Price,
                TimestampCreated = order.TimestampCreated
            };



            return orderDto;
        }

        public IEnumerable<OrderDto> GetOrdersByCustomer(Customer customer)
        {
            var orders = _dbContext.Orders
        .Where(o => o.Customer.CustomerId == customer.CustomerId)
        .Include(o => o.Customer)
        .Include(o => o.Cart)
        .ThenInclude(cart => cart.OrderItems)
        .ToList();

            var orderDtos = orders.Select(order => new OrderDto
            {
                OrderId = order.OrderId,
                Customer = new CustomerDto
                {
                    UserName = order.Customer.Name,
                },
                Cart = new CartDto
                {
                    CartId = order.Cart.CartId,
                    Price = order.Cart.Price,
                    OrderItems = order.Cart.OrderItems.Select(item => new OrderItemDto
                    {
                        OrderItemId = item.OrderItemId,
                        FoodName = _foodDeliveryService.GetFoodNameById(item.FoodId),
                        Pieces = item.Pieces,
                        Price = item.Price
                    }).ToList()
                },
                Price = order.Cart.Price,
                TimestampCreated = order.TimestampCreated
            });

            return orderDtos;
        }
    }
}