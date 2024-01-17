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

        public Food? GetFoodByName(string name)
        {
            Food matchingFood = _dbContext.Foods
        .FirstOrDefault(f => f.Name.ToLower() == name.ToLower());
            return matchingFood;
        }

        public string GetFoodNameById(long foodId)
        {

            Console.WriteLine("FOOD ID - " + foodId);
            Food matchingFood = _dbContext.Foods
                .FirstOrDefault(f => f.FoodId == foodId);
            Console.WriteLine("FOOD NAME - " + matchingFood);

            return matchingFood?.Name;
        }

        public Food AddFood(FoodDto newFood)
        {
            if (newFood == null)
            {
                throw new ArgumentNullException(nameof(newFood));
            }
            if (_dbContext.Foods.Any(f => f.Name == newFood.Name))
            {
                throw new InvalidOperationException("Food with the same name already exists");
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
    }
}