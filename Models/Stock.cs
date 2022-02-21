namespace RestaurantRecipeManager.Models
{
    public class Stock
    {
        public byte IId { get; set; }
        public byte Quantity { get; set; }

        public Ingredient IIdNavigation { get; set; }
    }
}