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
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        [ForeignKey("CartId")]
        public Cart Cart{ get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public DateTime TimestampCreated { get; set; }


        public Order()
        {
        }

        public Order(Customer customer)
        {
            Customer = customer;
            Cart = customer.Cart;
            Price = customer.Cart.Price;
            TimestampCreated = DateTime.Now;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Order otherOrder = (Order)obj;
            return OrderId == otherOrder.OrderId && Customer.CustomerId == otherOrder.Customer.CustomerId && Cart.OrderItems.SequenceEqual(otherOrder.Cart.OrderItems) && Price == otherOrder.Price && TimestampCreated == otherOrder.TimestampCreated;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(OrderId, Customer.CustomerId, Cart.OrderItems, Price, TimestampCreated);
        }
    }
}