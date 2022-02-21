using System;

namespace RestaurantRecipeManager.Models
{
    public class Reservation
    {
        public byte ResId { get; set; }
        public byte TId { get; set; }
        public DateTime Date { get; set; }
        public byte LId { get; set; }

        public Login LIdNavigation { get; set; }
        public Table TIdNavigation { get; set; }
    }
}