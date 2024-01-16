namespace food_delivery.Dto
{
    public class AddToCartDto
    {
        public string UserName { get; set; }
        public long FoodId { get; set; }
        public int Quantity { get; set; }
    }
}