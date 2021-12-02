using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CafeLab.Models
{
    public partial class Cafe
    {
        public Cafe()
        {
            Orders = new HashSet<Order>();
        }

        public int CafeId { get; set; }

        [Required(ErrorMessage="Це поле є обов'язковим!")]
        [Display(Name = "Адреса")]
        [Remote("DoesCafeAlreadyExists", "Cafes", ErrorMessage = "Кафе з такою адресою вже існує, спробуйте іншу!")]
        public string Address { get; set; }

        [Display(Name = "Час відкриття")]
        [Required(ErrorMessage = "Це поле є обов'язковим!")]
        public TimeSpan OpenHour { get; set; }

        [Display(Name = "Час закриття")]
        [Required(ErrorMessage = "Це поле є обов'язковим!")]
        public TimeSpan CloseHour { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
