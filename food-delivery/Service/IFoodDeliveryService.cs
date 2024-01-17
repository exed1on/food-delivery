using food_delivery.Domain;
using food_delivery.Dto;

namespace food_delivery.Service
{
    public interface IFoodDeliveryService
    {
        List<Food> ListAllFood();
    
        Food AddFood(FoodDto food);
        Food UpdateFood(FoodDto updatedFood);
        Food GetFoodByName(string name);
        string DeleteFood(long foodId);
        string GetFoodNameById(long foodId);
    }
}