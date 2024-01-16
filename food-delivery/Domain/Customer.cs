using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace food_delivery.Domain
{
    public class Customer : Credentials
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CustomerId { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Balance { get; set; }
        public long CartId { get; set; }

        [ForeignKey("CartId")]
        public Cart Cart { get; set; }
        public ICollection<Order> Orders { get; set; }

        public Customer()
        {
        }

        public Customer(string userName, string password, string name, decimal balance, Cart cart)
            : base(userName, password)
        {
            UserName = userName;
            Password = password;
            Name = name;
            Balance = balance;
            Cart = cart;
            Orders = new List<Order>();
            CartId = cart.CartId;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Customer otherCustomer = (Customer)obj;
            return CustomerId == otherCustomer.CustomerId && base.Equals(obj) && Name == otherCustomer.Name && Balance == otherCustomer.Balance && Cart.Equals(otherCustomer.Cart) && Orders.SequenceEqual(otherCustomer.Orders);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CustomerId, base.GetHashCode(), Name, Balance, Cart, Orders);
        }
    }
}