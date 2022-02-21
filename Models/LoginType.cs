using System.Collections.Generic;

namespace RestaurantRecipeManager.Models
{
    public class LoginType
    {
        public LoginType()
        {
            Logins = new HashSet<Login>();
        }

        public byte TypeId { get; set; }
        public string Name { get; set; }
        public byte Chmod { get; set; }

        public IEnumerable<Login> Logins { get; set; }
    }
}