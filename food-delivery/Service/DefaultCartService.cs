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

        public Cart AddFoodToCart(Customer customer, Food food, int quantity)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            var cart = GetCartById(customer.Cart.CartId);

            if (cart == null)
            {
                Console.WriteLine("CART WAS NULL");
                cart = new Cart();
                customer.Cart = cart;
                _dbContext.Carts.Add(cart);
            }

            var cartItem = cart.OrderItems.FirstOrDefault(ci => ci.Food.FoodId == food.FoodId);

            if (cartItem == null)
            {
                cartItem = new OrderItem(food, quantity, food.Price);

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
                cartItem.Pieces += quantity;
                _dbContext.OrderItems.Update(cartItem);
            }

            cart.Price = cart.OrderItems.Sum(item => item.Price * item.Pieces);

            _dbContext.Carts.Update(cart);
            _dbContext.Customers.Update(customer);

            _dbContext.SaveChanges();
            Console.WriteLine("AFTER ");

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
            }
        }

        public List<OrderItem> GetCartItems(Cart cart)
        {
            return cart.OrderItems.ToList();
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
                    _dbContext.Carts.Update(cart);

                    _dbContext.SaveChanges();
                }
            }
        }

    }
}