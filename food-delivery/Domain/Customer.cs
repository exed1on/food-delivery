using food_delivery.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

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

        [ForeignKey("CartId")]
        public Cart Cart { get; set; }

        [JsonIgnore]
        public ICollection<Order> Orders { get; set; }

        [Required]
        public Roles Role{ get; set; }

        public Customer()
        {
        }

        public Customer(string userName, string password, string name, decimal balance, Cart cart, Roles role)
            : base(userName, password)
        {
            UserName = userName;
            Password = password;
            Name = name;
            Balance = balance;
            Cart = cart;
            Orders = new List<Order>();
            Role = role;
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