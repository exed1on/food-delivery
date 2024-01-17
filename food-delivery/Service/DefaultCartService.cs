using System;
using System.Security.Authentication;
using food_delivery.Domain;
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

        public void AddFoodToCart(Cart cart, Food food, int quantity)
        {
            var cartItem = cart.OrderItems.FirstOrDefault(ci => ci.FoodId == food.FoodId);

            if (cartItem == null)
            {
                cartItem = new OrderItem(food, quantity, food.Price);
                cart.OrderItems.Add(cartItem);
                cart.Price += food.Price * quantity;
            }
            else
            {
                cartItem.Pieces += quantity;
                cart.Price += food.Price * (quantity - cartItem.Pieces);
            }

            _dbContext.SaveChanges();
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
            var cartItem = cart.OrderItems.FirstOrDefault(ci => ci.FoodId == food.FoodId);

            if (cartItem != null)
            {
                cart.OrderItems.Remove(cartItem);
                _dbContext.SaveChanges();
            }
        }

        public List<OrderItem> GetCartItems(Cart cart)
        {
            return cart.OrderItems.ToList();
        }

        public void UpdateCartItemQuantity(Cart cart, Food food, int newQuantity)
        {
            var cartItem = cart.OrderItems.FirstOrDefault(ci => ci.FoodId == food.FoodId);

            if (cartItem != null)
            {
                cartItem.Pieces = newQuantity;
                _dbContext.SaveChanges();
            }
        }

        public void ClearCart(string userName)
        {
            var customer = _dbContext.Customers.SingleOrDefault(c => c.UserName == userName);
            if (customer != null)
            {
                var cart = _dbContext.Carts.SingleOrDefault(c => c.CartId == customer.CartId);
                if (cart != null)
                {
                    cart.OrderItems = new List<OrderItem>();
                    _dbContext.SaveChanges();
                }
            }
        }

    }
}