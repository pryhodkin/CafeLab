using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CafeLab.Models
{
    public partial class Category
    {
        public Category()
        {
            Dishes = new HashSet<Dish>();
        }

        public int CategoryId { get; set; }

        [Remote("DoesCategoryAlreadyExists", "Categories", ErrorMessage = "Категорія з такою назвою вже існує, спробуйте іншу!")]
        [Required(ErrorMessage = "Це поле є обов'язковим!")]
        [Display(Name = "Назва")]
        public string Name { get; set; }

        [Display(Name = "Опис")]
        public string Description { get; set; }

        public virtual ICollection<Dish> Dishes { get; set; }
    }
}
