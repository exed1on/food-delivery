using System;
using System.Security.Authentication;
using food_delivery.Domain;
using food_delivery.Dto;
using food_delivery.Service;
using Microsoft.EntityFrameworkCore;

namespace food_delivery.Service
{
    public class DefaultCartService : ICartService
    {
        private readonly List<Food> foodList;
        private readonly List<Customer> customerList;
        private readonly AppDbContext _dbContext;

        public DefaultCartService()
        {

        }
        public DefaultCartService(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Cart AddFoodToCart(AddToCartDto addToCartDto)
        {
            if (addToCartDto == null)
            {
                throw new ArgumentNullException(nameof(addToCartDto));
            }

            var customer = _dbContext.Customers.Include(c => c.Cart).SingleOrDefault(c => c.UserName == addToCartDto.UserName);
            if (customer == null)
            {
                throw new InvalidOperationException("Customer not found");
            }
            Console.WriteLine(customer.UserName);
            var cart = GetCartById(customer.Cart.CartId);

            if (cart == null)
            {
                Console.WriteLine("CART WAS NULL");
                cart = new Cart();
                customer.Cart = cart;
                _dbContext.Carts.Add(cart);
            }
            Console.WriteLine(cart.ToString());

            var food = _dbContext.Foods.SingleOrDefault(f => f.Name == addToCartDto.FoodName);

            if (food == null)
            {
                throw new InvalidOperationException("Food not found");
            }

            Console.WriteLine(food.Name);

            var cartItem = cart.OrderItems.FirstOrDefault(ci => ci.Food.FoodId == food.FoodId);

            if (cartItem == null)
            {
                cartItem = new OrderItem(food, addToCartDto.Quantity, food.Price);

                if (cart.OrderItems == null)
                {
                    cart.OrderItems = new List<OrderItem>();
                    _dbContext.Entry(cart).Collection(c => c.OrderItems).IsModified = true;
                }

                cart.OrderItems.Add(cartItem);
                _dbContext.OrderItems.Add(cartItem);
            }
            else
            {
                cartItem.Price = food.Price;
                cartItem.Pieces += addToCartDto.Quantity;
                _dbContext.OrderItems.Update(cartItem);
            }

            cart.Price = cart.OrderItems.Sum(item => item.Price * item.Pieces);

            _dbContext.Carts.Update(cart);
            _dbContext.Customers.Update(customer);

            _dbContext.SaveChanges();
            Console.WriteLine("AFTER");

            return customer.Cart;
        }


        public Food GetFoodById(long foodId)
        {
            var food = _dbContext.Foods.FirstOrDefault(f => f.FoodId == foodId);

            if (food == null)
            {
                throw new NullReferenceException($"Food with ID {foodId} not found.");
            }

            return food;
        }

        public Cart GetCartById(long cartId)
        {
            var cart = _dbContext.Carts
                .Include(c => c.OrderItems)
                .ThenInclude(ci => ci.Food)
                .FirstOrDefault(c => c.CartId == cartId);

            if(cart == null)
            {
                throw new NullReferenceException(nameof(cart));
            }

            return cart;
        }

        public void RemoveFoodFromCart(Cart cart, Food food)
        {
            var cartItem = cart.OrderItems.FirstOrDefault(ci => ci.Food.FoodId == food.FoodId);


            if (cartItem != null)
            {
                cart.OrderItems.Remove(cartItem);
                _dbContext.SaveChanges();
                cart.Price -= food.Price * cartItem.Pieces;
                _dbContext.Update(cart);
                _dbContext.SaveChanges();
            }
        }

        public List<OrderItem> GetCartItems(Cart cart)
        {
            var sortedOrderItems = cart.OrderItems.OrderBy(item => item.Food.Name).ToList();
            return sortedOrderItems;
        }

        public void UpdateCartItemQuantity(Cart cart, Food food, int newQuantity)
        {
            var cartItem = cart.OrderItems.FirstOrDefault(ci => ci.Food.FoodId == food.FoodId);

            if (cartItem != null)
            {
                cartItem.Pieces = newQuantity;
                cart.Price = cart.OrderItems.Sum(item => item.Price * item.Pieces);
                _dbContext.SaveChanges();
            }
        }

        public void ClearCart(string userName)
        {
            var customer = _dbContext.Customers.SingleOrDefault(c => c.UserName == userName);

            if (customer != null)
            {
                _dbContext.Entry(customer).Reference(c => c.Cart).Load();

                var cart = customer.Cart;

                if (cart != null)
                {
                    
                    foreach (var orderItem in _dbContext.OrderItems.ToList())
                    {
                        Console.WriteLine("---" + orderItem);
                        _dbContext.OrderItems.Remove(orderItem);
                    }

                    cart.OrderItems.Clear();
                    cart.Price = 0;
                    _dbContext.Carts.Update(cart);

                    _dbContext.SaveChanges();
                }
            }
        }

    }
}