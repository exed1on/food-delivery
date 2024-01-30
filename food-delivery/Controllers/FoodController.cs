using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using food_delivery.Domain;
using food_delivery.Service;
using food_delivery.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace food_delivery.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoodController : ControllerBase
    {
        private readonly IFoodDeliveryService _foodDeliveryService;

        public FoodController(IFoodDeliveryService foodDeliveryService, AppDbContext dbContext)
        {
            Console.WriteLine("FOODCONTROLLER INITIALIZING");
            _foodDeliveryService = foodDeliveryService;
     
        }

        [HttpGet("listAllFood/")]
        public List<Food> GetListOfFood()
        {
            var food = _foodDeliveryService.ListAllFood();

            return food;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("addFood")]
        public ActionResult<Food> AddFood([FromBody] FoodDto newFood)
        {
            if (newFood == null)
            {
                throw new ArgumentNullException(nameof(newFood), "The 'newFood' parameter cannot be null.");
            }

            var addedFood = _foodDeliveryService.AddFood(newFood);

            return addedFood;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("updateFood")]
        public ActionResult<Food> UpdateExistingFood([FromBody] FoodDto updatedFood)
        {
            if (updatedFood == null)
            {
                return BadRequest("The 'updatedFood' parameter cannot be null.");
            }

            var foodAfterUpdate = _foodDeliveryService.UpdateFood(updatedFood);

            return foodAfterUpdate;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("deleteFood/{foodNameToDelete}")]
        public ActionResult<string> DeleteFood(string foodNameToDelete)
        {
            var existingFood = _foodDeliveryService.DeleteFood(foodNameToDelete);        
            return "Food was successfully deleted";
        }

        [Authorize(Roles = "ADMIN")]
        [HttpHead("/api/Food/rights")]
        public IActionResult CheckCustomer()
        {
            return Ok();
        }
    }
}