using System;
using System.Collections.Generic;

#nullable disable

namespace CafeLab.Models
{
    public partial class DishesInOrder
    {
        public int DishInOrderId { get; set; }
        public int DishId { get; set; }
        public int OrderId { get; set; }
        public int Quantity { get; set; }

        public virtual Dish Dish { get; set; }
        public virtual Order Order { get; set; }
    }
}
