using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CafeLab.Models
{
    public partial class Dish
    {
        public Dish()
        {
            DishesInOrders = new HashSet<DishesInOrder>();
        }

        public int DishId { get; set; }

        [Remote("DoesDishAlreadyExists", "Dishes", ErrorMessage = "Страва з такою назвою вже існує, спробуйте іншу!")]
        [Required(ErrorMessage = "Це поле є обов'язковим!")]
        [Display(Name = "Назва")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Це поле є обов'язковим!")]
        [Display(Name = "Категорія")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Це поле є обов'язковим!")]
        [Display(Name = "Ціна")]
        [Range(0, int.MaxValue, ErrorMessage = "Ціна не може бути від'ємною!")]
        public int Price { get; set; }

        [Display(Name = "Вага")]
        [Range(0, int.MaxValue, ErrorMessage = "Вага не може бути від'ємною!")]
        public int? Weight { get; set; }

        [Display(Name = "Опис")]
        public string Description { get; set; }

        [Display(Name = "Фото")]
        public string ImageUrl { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<DishesInOrder> DishesInOrders { get; set; }
    }
}
