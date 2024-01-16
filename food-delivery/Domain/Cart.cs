using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public Cart()
        {
            OrderItems = new List<OrderItem>();
        }

        public Cart(List<OrderItem> orderItems)
        {
            OrderItems = orderItems;
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
    }
}