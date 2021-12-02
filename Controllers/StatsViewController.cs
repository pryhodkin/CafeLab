using CafeLab.Models;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CafeLab.Controllers
{

    public class StatsViewController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        DBCafeContext _context = new DBCafeContext();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);
                        try
                        {
                            using (XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                            {

                                foreach (IXLWorksheet worksheet in workBook.Worksheets)
                                {

                                    Category newcat;
                                    var c = (from cat in _context.Categories
                                             where cat.Name.Contains(worksheet.Name)
                                             select cat).ToList();
                                    if (c.Count > 0)
                                    {
                                        newcat = c[0];
                                    }
                                    else
                                    {
                                        newcat = new Category();
                                        newcat.Name = worksheet.Name;
                                        newcat.Description = "from EXCEL";

                                        _context.Categories.Add(newcat);
                                    }

                                    foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                    {
                                        try
                                        {
                                            Dish dish = new Dish();
                                            dish.Name = row.Cell(1).Value.ToString();
                                            dish.Price = Convert.ToInt32(row.Cell(2).Value.ToString());
                                            dish.Weight = Convert.ToInt32(row.Cell(3).Value.ToString());
                                            dish.Description = row.Cell(4).Value.ToString();
                                            dish.ImageUrl = row.Cell(5).Value.ToString();
                                            dish.Category = newcat;

                                            var results = new List<ValidationResult>();
                                            var context = new ValidationContext(dish, null, null);
                                            if (Validator.TryValidateObject(dish, context, results, true))
                                            {
                                                _context.Dishes.Add(dish);
                                            }

                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine(e.Message);

                                        }
                                    }
                                }
                            }
                        }
                        catch
                        {
                            await Response.WriteAsync("<script>window.location = 'index';alert('Uploaded file of unsupported format!');</script>");
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<ActionResult> Export()
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var dishes = await _context.Dishes.Include("Category").ToListAsync();

                var worksheet = workbook.Worksheets.Add("Меню");

                worksheet.Cell("A1").Value = "Назва";
                worksheet.Cell("B1").Value = "Ціна";
                worksheet.Cell("C1").Value = "Вага/місткість";
                worksheet.Cell("D1").Value = "Картинка";
                worksheet.Cell("E1").Value = "Категорія";
                worksheet.Cell("F1").Value = "Опис";
                worksheet.Row(1).Style.Font.Bold = true;

                for (int i = 2; i < dishes.Count() + 2; i++)
                {
                    worksheet.Cell(i, 1).Value = dishes[i - 2].Name;
                    worksheet.Cell(i, 2).Value = dishes[i - 2].Price;
                    worksheet.Cell(i, 3).Value = dishes[i - 2].Weight;
                    worksheet.Cell(i, 4).Value = dishes[i - 2].ImageUrl;
                    worksheet.Cell(i, 5).Value = dishes[i - 2].Category.Name;
                    worksheet.Cell(i, 6).Value = dishes[i - 2].Description;
                }


                var cafes = await _context.Cafes.ToListAsync();

                worksheet = workbook.Worksheets.Add("Кафе");

                worksheet.Cell("A1").Value = "Адреса";
                worksheet.Cell("B1").Value = "Час відкриття";
                worksheet.Cell("C1").Value = "Час закриття";
                worksheet.Row(1).Style.Font.Bold = true;

                for (int i = 2; i < cafes.Count() + 2; i++)
                {
                    worksheet.Cell(i, 1).Value = cafes[i - 2].Address;
                    worksheet.Cell(i, 2).Value = cafes[i - 2].OpenHour.ToString("hh\\:mm\\:ss");
                    worksheet.Cell(i, 3).Value = cafes[i - 2].CloseHour.ToString("hh\\:mm\\:ss");
                }

                var categories = await _context.Categories.ToListAsync();

                worksheet = workbook.Worksheets.Add("Категорії");

                worksheet.Cell("A1").Value = "Назва";
                worksheet.Cell("B1").Value = "Опис";
                worksheet.Row(1).Style.Font.Bold = true;

                for (int i = 2; i < categories.Count() + 2; i++)
                {
                    worksheet.Cell(i, 1).Value = categories[i - 2].Name;
                    worksheet.Cell(i, 2).Value = categories[i - 2].Description;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"cafe_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }

        public bool ValidateDish(Dish dish)
        {
            return true;
        }
    }
}
