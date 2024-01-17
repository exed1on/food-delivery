using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace food_delivery.Domain
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CartId { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } 
        [Required]
        public decimal Price { get; set; }

        public Cart(decimal price, List<OrderItem> orderItems)
        {
            Price = price;
            OrderItems = orderItems;
        }
        public Cart() 
        {
            OrderItems = new List<OrderItem>();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Cart otherCart = (Cart)obj;
            return CartId == otherCart.CartId && OrderItems.SequenceEqual(otherCart.OrderItems);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CartId, OrderItems);
        }

        internal static Cart GetEmptyCart()
        {
            return new Cart();
        }
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"CartId: {CartId}");
            stringBuilder.AppendLine($"Price: {Price}");

            if (OrderItems.Count > 0)
            {
                stringBuilder.AppendLine("Order Items:");
                foreach (var orderItem in OrderItems)
                {
                    stringBuilder.AppendLine($"  OrderItemId: {orderItem.OrderItemId}, FoodId: {orderItem.Food.FoodId}, Pieces: {orderItem.Pieces}, Price: {orderItem.Price}");
                }
            }
            else
            {
                stringBuilder.AppendLine("No Order Items in the cart.");
            }

            return stringBuilder.ToString();
        }
    }
}