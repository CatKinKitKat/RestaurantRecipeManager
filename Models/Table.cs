using System.Collections.Generic;

namespace RestaurantRecipeManager.Models
{
    public class Table
    {
        public Table()
        {
            Reservations = new HashSet<Reservation>();
            TbOrLists = new HashSet<TbOrList>();
        }

        public byte TId { get; set; }
        public string Name { get; set; }

        public IEnumerable<Reservation> Reservations { get; set; }
        public IEnumerable<TbOrList> TbOrLists { get; set; }
    }
}