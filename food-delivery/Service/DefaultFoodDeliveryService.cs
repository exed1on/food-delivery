using System;
using System.Security.Authentication;
using food_delivery.Domain;
using food_delivery.Dto;
using food_delivery.Service;
using Microsoft.EntityFrameworkCore;

namespace food_delivery.Service
{
    public class DefaultFoodDeliveryService : IFoodDeliveryService
    {
        private readonly List<Food> foodList;
        private readonly List<Customer> customerList;
        private readonly AppDbContext _dbContext;

        public DefaultFoodDeliveryService()
        {

        }
        public DefaultFoodDeliveryService(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Customer Authenticate(Credentials credentials)
        {
            var existingCustomer = customerList.FirstOrDefault(customer =>
                credentials.UserName == customer.UserName && credentials.Password == customer.Password);

            if (existingCustomer != null)
                return existingCustomer;

            throw new AuthenticationException("Incorrect credentials");
        }

        public List<Food> ListAllFood()
        {
            return _dbContext.Foods.ToList();
        }

        public void UpdateCart(Customer customer, Food food, int pieces)
        {
            if (pieces < 0)
                throw new ArgumentException("Amount of food must not be negative");

            Cart cart = customer.Cart;

            if (decimal.Add(cart.Price, food.Price * pieces) > customer.Balance)
                throw new LowBalanceException("This order exceeds your current balance");

            var orders = cart.OrderItems;

            if (orders.Any(x => x.Food.Equals(food)))
            {
                var existingOrderItem = orders.First(x => x.Food.Equals(food));

                if (pieces == 0)
                {
                    cart.Price -= existingOrderItem.Food.Price * existingOrderItem.Pieces;
                    orders.Remove(existingOrderItem);
                    cart.OrderItems = orders;
                    customer.Cart = cart;
                    return;
                }

                int indexOfOrderItem = orders.ToList().FindIndex(x => x.Equals(existingOrderItem));
                orders.ToList()[indexOfOrderItem] = new OrderItem(food, pieces, food.Price);
                cart.Price += food.Price * (pieces - existingOrderItem.Pieces);
                cart.OrderItems = orders;
                customer.Cart = cart;
                return;
            }
            else
            {
                if (pieces == 0)
                    throw new ArgumentException("You cannot add an item in amount of 0");
            }

            orders.Add(new OrderItem(food, pieces, food.Price));
            cart.OrderItems = orders;
            cart.Price += food.Price * pieces;
            customer.Cart = cart;
        }

        public Order CreateOrder(Customer customer)
        {
            var cart = customer.Cart;

            if (cart.OrderItems == null || cart.OrderItems.Count == 0)
                throw new InvalidOperationException("Cart cannot be empty");

            var order = new Order(customer);
            customer.Balance -= order.Price;
            customer.Cart = Cart.GetEmptyCart();
            return order;
        }

        public Food AddFood(FoodDto newFood)
        {
            if (newFood == null)
            {
                throw new ArgumentNullException(nameof(newFood));
            }
            var foodToAdd = new Food(newFood.Name, newFood.Calorie, newFood.Description, newFood.Price);

            var addedFood = _dbContext.Foods.Add(foodToAdd).Entity;
            _dbContext.SaveChanges();

            return addedFood;
        }

        public Food UpdateFood(FoodDto updatedFood)
        {
            if (updatedFood == null)
            {
                throw new ArgumentNullException(nameof(updatedFood));
            }

            var existingFood = _dbContext.Foods.Find(updatedFood.Name);

            if (existingFood == null)
            {
                throw new InvalidOperationException("Food not found");
            }

            existingFood.Calorie = updatedFood.Calorie;
            existingFood.Description = updatedFood.Description;
            existingFood.Price = updatedFood.Price;

            _dbContext.SaveChanges();

            return existingFood;
        }

        public string DeleteFood(long foodIdToDelete)
        {

            var existingFood = _dbContext.Foods.FirstOrDefault(f => f.FoodId == foodIdToDelete);

            if (existingFood == null)
            {
                return "There is no food with this id";
            }

            _dbContext.Foods.Remove(existingFood);
            _dbContext.SaveChanges();

            return "Food with id \"" + foodIdToDelete + "\" was succesfully deleted";
        }
        public bool CheckFoodByName(string foodName)
        {
            return _dbContext.Foods.Any(f => f.Name == foodName);
        }

    }
}