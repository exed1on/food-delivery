using System.Text.Json.Serialization;

namespace food_delivery.Dto
{
    public class OrderDto
    {
        public long OrderId { get; set; }
        public CustomerDto Customer { get; set; }
        public CartDto Cart { get; set; }
        public decimal Price { get; set; }
        public DateTime TimestampCreated { get; set; }
    }
}