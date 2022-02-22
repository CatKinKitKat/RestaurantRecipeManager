using System.Collections.Generic;

namespace RestaurantRecipeManager.API.Models
{
    public class RecipeModel
    {
        public byte RId { get; set; }
        public string Name { get; set; }

        public class Ingredient
        {
            public byte IId { get; set; }
            public byte Quantity { get; set; }

            public Ingredient(byte IId, byte Quantity)
            {
                this.IId = IId;
                this.Quantity = Quantity;
            }
        }

        public IEnumerable<Ingredient> Ingredients { get; set; }
    }
}