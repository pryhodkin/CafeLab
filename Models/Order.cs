using System;
using System.Collections.Generic;

#nullable disable

namespace CafeLab.Models
{
    public partial class Order
    {
        public Order()
        {
            DishesInOrders = new HashSet<DishesInOrder>();
        }

        public int OrderId { get; set; }
        public DateTime Datetime { get; set; }
        public string Address { get; set; }
        public int? SalecardId { get; set; }
        public int CafeId { get; set; }

        public virtual Cafe Cafe { get; set; }
        public virtual SaleCard Salecard { get; set; }
        public virtual ICollection<DishesInOrder> DishesInOrders { get; set; }
    }
}
