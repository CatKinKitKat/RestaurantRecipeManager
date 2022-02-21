using System.Collections.Generic;

namespace RestaurantRecipeManager.Models
{
    public class Ingredient
    {
        public Ingredient()
        {
            RecIngLists = new HashSet<RecIngList>();
        }

        public byte IId { get; set; }
        public string Name { get; set; }

        public Stock Stock { get; set; }
        public IEnumerable<RecIngList> RecIngLists { get; set; }
    }
}