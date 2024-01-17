using food_delivery.Domain;
using food_delivery.Dto;

namespace food_delivery.Service
{
    public interface IOrderService
    {
        OrderDto CreateOrder(Customer customer);
        IEnumerable<OrderDto> GetOrdersByCustomer(Customer customer);
    }
}