namespace food_delivery.Dto
{
    public class OrderItemDto
    {
        public long OrderItemId { get; set; }
        public string FoodName { get; set; }
        public long Pieces { get; set; }
        public decimal Price { get; set; }
    }
}