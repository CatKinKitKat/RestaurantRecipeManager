namespace RestaurantRecipeManager.Models
{
    public class TbOrList
    {
        public byte TolId { get; set; }
        public byte OId { get; set; }
        public byte TId { get; set; }
        public byte Quantity { get; set; }

        public Order OIdNavigation { get; set; }
        public Table TIdNavigation { get; set; }
    }
}