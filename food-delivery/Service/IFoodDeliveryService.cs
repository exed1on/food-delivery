using food_delivery.Domain;

namespace food_delivery.Service
{
    public interface IFoodDeliveryService
    {
        List<Food> ListAllFood();

        void UpdateCart(Customer customer, Food food, int pieces);
    
        Order CreateOrder(Customer customer);
        Food AddFood(Food food);
        Food UpdateFood(Food updatedFood);
        Food DeleteFood(string foodName);
        bool CheckFoodByName(string foodName);
    }
}