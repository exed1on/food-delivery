using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace food_delivery.Domain
{
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OrderItemId { get; set; }

        public long FoodId { get; set; }

        [ForeignKey("FoodId")]
        public Food Food { get; set; }

        [Required]
        public long Pieces { get; set; }

        [Required]
        public decimal Price { get; set; }

        public OrderItem()
        {
        }

        public OrderItem(Food food, int pieces, decimal price)
        {
            Food = food;
            Pieces = pieces;
            Price = price;
        }

        public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var otherItem = (OrderItem)obj;

        return Food.Equals(otherItem.Food) &&
               Pieces == otherItem.Pieces &&
               Price == otherItem.Price;
    }

        public override int GetHashCode()
        {
            return HashCode.Combine(OrderItemId, Food, Pieces, Price);
        }
    }
}