using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace food_delivery.Domain
{
    public class Food
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FoodId { get; set; }

        [Required]
        public string Name { get; set; }

        public decimal Calorie { get; set; }

        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public Food()
        {
        }

        public Food(string name, decimal calorie, string description, decimal price)
        {
            Name = name;
            Calorie = calorie;
            Description = description;
            Price = price;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Food otherFood = (Food)obj;
            return FoodId == otherFood.FoodId && Name == otherFood.Name && Calorie == otherFood.Calorie && Description == otherFood.Description && Price == otherFood.Price;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FoodId, Name, Calorie, Description, Price);
        }
    }
}