using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace food_delivery.Domain
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OrderId { get; set; }

        [Required]
        public long CustomerId { get; set; }

        public Customer Customer { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public DateTime TimestampCreated { get; set; }

        public Order()
        {
        }

        public Order(Customer customer)
        {
            CustomerId = customer.CustomerId;
            OrderItems = customer.Cart.OrderItems;
            TimestampCreated = DateTime.Now;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Order otherOrder = (Order)obj;
            return OrderId == otherOrder.OrderId && CustomerId == otherOrder.CustomerId && OrderItems.SequenceEqual(otherOrder.OrderItems) && Price == otherOrder.Price && TimestampCreated == otherOrder.TimestampCreated;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(OrderId, CustomerId, OrderItems, Price, TimestampCreated);
        }
    }
}