using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using food_delivery.Domain;
using food_delivery.Service;
using Microsoft.EntityFrameworkCore;

namespace food_delivery.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoodController : ControllerBase
    {
        private readonly IFoodDeliveryService _foodDeliveryService;
        private readonly AppDbContext _dbContext;

        public FoodController(IFoodDeliveryService foodDeliveryService, AppDbContext dbContext)
        {
            Console.WriteLine("FOODCONTROLLER INITIALIZING");
            _dbContext = dbContext;
            _foodDeliveryService = foodDeliveryService;
     
        }

        [HttpGet("listAllFood/")]
        public List<Food> GetListOfFood()
        {
            var food = _foodDeliveryService.ListAllFood();

            return food;
        }

        [HttpPost("addFood")]
        public ActionResult<Food> AddFood([FromBody] Food newFood)
        {
            if (newFood == null)
            {
                throw new ArgumentNullException(nameof(newFood), "The 'newFood' parameter cannot be null.");
            }

            var addedFood = _dbContext.Foods.Add(newFood).Entity;
            _dbContext.SaveChanges();

            return addedFood;
        }
        [HttpPut("updateFood")]
        public ActionResult<Food> UpdateExistingFood([FromBody] Food updatedFood)
        {
            if (updatedFood == null)
            {
                return BadRequest("The 'updatedFood' parameter cannot be null.");
            }

            var existingFood = _dbContext.Foods.FirstOrDefault(f => f.FoodId == updatedFood.FoodId);

            if (existingFood == null)
            {
                return NotFound("Food not found");
            }

            existingFood.Name = updatedFood.Name;
            existingFood.Calorie = updatedFood.Calorie;
            existingFood.Description = updatedFood.Description;
            existingFood.Price = updatedFood.Price;

            _dbContext.SaveChanges();

            return existingFood;
        }

        [HttpDelete("deleteFood")]
        public ActionResult<Food> DeleteFood([FromBody] Food foodToDelete)
        {
            if (foodToDelete == null)
            {
                return BadRequest("The 'foodToDelete' parameter cannot be null.");
            }

            var existingFood = _dbContext.Foods.FirstOrDefault(f => f.FoodId == foodToDelete.FoodId);

            if (existingFood == null)
            {
                return NotFound("Food not found");
            }

            _dbContext.Foods.Remove(existingFood);
            _dbContext.SaveChanges();

            return existingFood;
        }

        [HttpHead("/{foodName}")]
        public bool CheckCustomer(string foodName)
        {
            var isCustomer = _foodDeliveryService.CheckFoodByName(foodName);
            return isCustomer;
        }
    }
}