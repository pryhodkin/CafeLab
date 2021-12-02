using System;
using System.Collections.Generic;

#nullable disable

namespace CafeLab.Models
{
    public partial class SaleCard
    {
        public SaleCard()
        {
            Orders = new HashSet<Order>();
        }

        public int SalecardId { get; set; }
        public int DiscountSum { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
