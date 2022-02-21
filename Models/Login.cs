using System.Collections.Generic;

namespace RestaurantRecipeManager.Models
{
    public class Login
    {
        public Login()
        {
            Reservations = new HashSet<Reservation>();
        }

        public byte LId { get; set; }
        public string Username { get; set; }
        public string Passhash { get; set; }
        public byte TypeId { get; set; }

        public LoginType Type { get; set; }
        public IEnumerable<Reservation> Reservations { get; set; }
    }
}