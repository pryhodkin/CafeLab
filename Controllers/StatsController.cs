using CafeLab.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CafeLab.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatsController : Controller
    {
        protected readonly DBCafeContext _context;

        public StatsController(DBCafeContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "admin")]
        [HttpGet(nameof(GetCategoriesData))]
        public JsonResult GetCategoriesData()
        {
            var data = _context.Categories
                               .Select(x => new object[]
                               {
                                   x.Name,
                                   x.Dishes.Count
                               }).ToList();

            data.Insert(0, new object[] { "Категорія", "Кількість страв" });

            return new JsonResult(data);

        }

        [Authorize(Roles = "admin")]
        [HttpGet(nameof(GetCafesData))]
        public JsonResult GetCafesData()
        {
            var data = _context
                       .Cafes
                       .GroupBy(x =>
                                    x.CloseHour.Hours > x.OpenHour.Hours ?
                                    x.CloseHour.Hours - x.OpenHour.Hours :
                                    x.CloseHour.Hours - x.OpenHour.Hours + 24
                                ).Select(x => new object[]
                                {
                                    x.Key.ToString(),
                                    x.Count()
                                })
                                .ToList();

            data.Insert(0, new object[] { "Кількість робочихгодин на добу", "Кількість кафе" });

            return new JsonResult(data);

        }
    }
}
