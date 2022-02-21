using System;
using System.Collections.Generic;

namespace RestaurantRecipeManager.Models
{
    public class Order
    {
        public Order()
        {
            TbOrLists = new HashSet<TbOrList>();
        }

        public byte OId { get; set; }
        public DateTime Date { get; set; }
        public byte RId { get; set; }

        public Recipe RIdNavigation { get; set; }
        public IEnumerable<TbOrList> TbOrLists { get; set; }
    }
}