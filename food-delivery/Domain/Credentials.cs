using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace food_delivery.Domain
{
    [Table("Customers")]
    public class Credentials
    {

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public Credentials()
        {
        }

        public Credentials(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Credentials other = (Credentials)obj;
            return UserName == other.UserName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(UserName);
        }
    }
}