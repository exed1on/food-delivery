using food_delivery.Domain;
using food_delivery.Dto;

namespace food_delivery.Service
{
    public interface IFoodDeliveryService
    {
        List<Food> ListAllFood();

        void UpdateCart(Customer customer, Food food, int pieces);
    
        Order CreateOrder(Customer customer);
        Food AddFood(FoodDto food);
        Food UpdateFood(FoodDto updatedFood);
        Food GetFoodByName(string name);
        string DeleteFood(long foodId);
    }
}