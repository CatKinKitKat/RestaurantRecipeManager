using System.Collections.Generic;

namespace RestaurantRecipeManager.Models
{
    public class Recipe
    {
        public Recipe()
        {
            Orders = new HashSet<Order>();
            RecIngLists = new HashSet<RecIngList>();
        }

        public byte RId { get; set; }
        public string Name { get; set; }

        public IEnumerable<Order> Orders { get; set; }
        public IEnumerable<RecIngList> RecIngLists { get; set; }
    }
}