using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using Shop.Data.Interfaces;
using Shop.Data.Models;
using Shop.Data.ViewModels;

namespace Shop.Controllers
{
    public class ItemsController : Microsoft.AspNetCore.Mvc.Controller
    {
        public IItems IAllItems;
        private ICategorys IAllCategorys;
        private readonly IWebHostEnvironment hostingEnvironment;
        VMItems VMItems = new VMItems();
        public ItemsController(IItems IAllItems, ICategorys IAllCategorys, IWebHostEnvironment environment)
        {
            this.IAllItems = IAllItems;
            this.IAllCategorys = IAllCategorys;
            this.hostingEnvironment = environment;
        }

        public ViewResult List(int id = 0)
        {
            ViewBag.Title = "Страница с предметами";
            VMItems.Items = IAllItems.AllItems;
            VMItems.Categorys = IAllCategorys.AllCategory;
            VMItems.SelectCategory = id;

            return View(VMItems);
        }

        [HttpGet]
        public ViewResult Add()
        {
            IEnumerable<Categorys> categorys = IAllCategorys.AllCategory;
            return View(categorys);
        }

        ///<summary>
        ///Метод добавления предмета
        ///</summary>
        ///<param name="name">Наименование предмета</рагат>
        ///<param name = "description" >Описание предмета</param>
        ///<param name = "files">Изображение </ param >
        ///<param name= "price">Цена </ param >
        ///<param name= "idCategory">Код категории</param>
        ///<returns></returns>
        [HttpPost]
        public IActionResult Add(string name, string descr, IFormFile files, float price, int idCategory)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return BadRequest("Наименование обязательно");

                string imgFileName = "";
                if (files != null && files.Length > 0)
                {
                    var uploads = Path.Combine(hostingEnvironment.WebRootPath, "img");
                    Directory.CreateDirectory(uploads);

                    var ext = Path.GetExtension(files.FileName).ToLowerInvariant();
                    if (!new[] { ".jpg", ".jpeg", ".png", ".gif" }.Contains(ext))
                        return BadRequest("Разрешены только изображения (.jpg, .png, .gif)");

                    imgFileName = Guid.NewGuid().ToString("N") + ext;
                    var filePath = Path.Combine(uploads, imgFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        files.CopyTo(stream);
                    }
                }

                var newItem = new Items
                {
                    Name = name,
                    Description = descr ?? "",
                    Img = imgFileName,
                    Price = (int)price,
                    Category = new Categorys { Id = idCategory }
                };

                int newId = IAllItems.Add(newItem);
                return RedirectToAction("List");
            }
            catch
            {
                return StatusCode(500, "Ошибка при сохранении");
            }
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Edit(int _id, string name, string descr, float price, int idCategory, string imageUrl)
        {
            try
            {
                var items = IAllItems.AllItems.FirstOrDefault(i => i.Id == _id);
                if (items == null)
                {
                    TempData["Error"] = $"Товар с ID {_id} не найден!";
                    return RedirectToAction("Delete");
                }

                var item = new Items
                {
                    Id = _id,
                    Name = name,
                    Description = descr,
                    Price = (int)price,
                    Category = new Categorys { Id = idCategory },
                    Img = string.IsNullOrEmpty(imageUrl) ? "/img/no-image.png" : imageUrl
                };
                IAllItems.Update(_id);
                TempData["Success"] = "Товар успешно обновлен!";
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Ошибка: {ex.Message}";
                return RedirectToAction("Delete");
            }
        }

        [HttpGet]
        public IActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                var item = IAllItems.AllItems.FirstOrDefault(i => i.Id == id);
                if (item == null)
                {
                    TempData["Error"] = $"Товар с ID {id} не найден!";
                    return RedirectToAction("Delete");
                }

                IAllItems.Delete(id);
                TempData["Success"] = $"Товар \"{item.Name}\" успешно удален!";
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Ошибка: {ex.Message}";
                return RedirectToAction("Delete");
            }
        }
    }
}

