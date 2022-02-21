namespace RestaurantRecipeManager.Models
{
    public class RecIngList
    {
        public byte RilId { get; set; }
        public byte RId { get; set; }
        public byte IId { get; set; }
        public byte Quantity { get; set; }

        public Ingredient IIdNavigation { get; set; }
        public Recipe RIdNavigation { get; set; }
    }
}