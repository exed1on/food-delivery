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
        [HttpDelete("deleteFood/{foodIdToDelete}")]
        public ActionResult<string> DeleteFood(long foodIdToDelete)
        {
            if (foodIdToDelete <= 0)
            {
                return BadRequest("The 'foodIdToDelete' parameter cannot be less or equal to 0");
            }

            var existingFood = _foodDeliveryService.DeleteFood(foodIdToDelete);

            return existingFood;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpHead("/api/Food/{foodName}")]
        public IActionResult CheckCustomer(string foodName)
        {
            var food = _foodDeliveryService.GetFoodByName(foodName);
            if(food != null)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}