namespace food_delivery.Dto
{
    public class CartDto
    {
        public long CartId { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        public decimal Price { get; set; }
    }
}