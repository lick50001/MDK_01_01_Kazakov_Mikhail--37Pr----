using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using Shop.Data.Interfaces;
using Shop.Data.Models;
using Shop.Data.ViewModels;

namespace Shop.Controllers
{
    public class ItemsController : Controller
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

        public ViewResult List(string searchString = "", int id = 0)
        {
            ViewBag.Title = "Страница с предметами";
            ViewBag.SearchString = searchString;

            var items = IAllItems.AllItems;

            if (!string.IsNullOrEmpty(searchString))
            {
                items = items.Where(x => x.Name.ToLower().Contains(searchString.ToLower()) ||
                                        (x.Description != null && x.Description.ToLower().Contains(searchString.ToLower())));
                ViewBag.SearchResult = items.Count();
            }

            VMItems.Items = items;
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

        [HttpPost]
        public IActionResult Add(string name, string descr, IFormFile files, float price, int idCategory)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    TempData["Error"] = "Наименование обязательно";
                    return RedirectToAction("Add");
                }

                string imgFileName = "";
                if (files != null && files.Length > 0)
                {
                    var uploads = Path.Combine(hostingEnvironment.WebRootPath, "img");
                    Directory.CreateDirectory(uploads);

                    var ext = Path.GetExtension(files.FileName).ToLowerInvariant();
                    if (!new[] { ".jpg", ".jpeg", ".png", ".gif" }.Contains(ext))
                    {
                        TempData["Error"] = "Разрешены только изображения (.jpg, .png, .gif)";
                        return RedirectToAction("Add");
                    }

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
                TempData["Success"] = "Товар успешно добавлен!";
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ошибка при сохранении: " + ex.Message;
                return RedirectToAction("Add");
            }
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var item = IAllItems.AllItems.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                TempData["Error"] = "Товар не найден!";
                return RedirectToAction("List");
            }

            ViewBag.Categories = IAllCategorys.AllCategory;
            return View(item);
        }

        [HttpPost]
        public IActionResult Update(int id, string name, string descr, float price, int idCategory, IFormFile files)
        {
            try
            {
                // Проверяем существование товара
                var existingItem = IAllItems.AllItems.FirstOrDefault(i => i.Id == id);
                if (existingItem == null)
                {
                    TempData["Error"] = "Товар не найден!";
                    return RedirectToAction("List");
                }

                // Валидация
                if (string.IsNullOrWhiteSpace(name))
                {
                    TempData["Error"] = "Наименование обязательно";
                    return RedirectToAction("Update", new { id = id });
                }

                if (price <= 0)
                {
                    TempData["Error"] = "Цена должна быть больше 0";
                    return RedirectToAction("Update", new { id = id });
                }

                string imgFileName = existingItem.Img;

                // Обработка нового изображения
                if (files != null && files.Length > 0)
                {
                    var uploads = Path.Combine(hostingEnvironment.WebRootPath, "img");
                    Directory.CreateDirectory(uploads);

                    var ext = Path.GetExtension(files.FileName).ToLowerInvariant();
                    if (!new[] { ".jpg", ".jpeg", ".png", ".gif" }.Contains(ext))
                    {
                        TempData["Error"] = "Разрешены только изображения";
                        return RedirectToAction("Update", new { id = id });
                    }

                    // Удаляем старое изображение
                    if (!string.IsNullOrEmpty(existingItem.Img) && existingItem.Img != "no-image.png")
                    {
                        var oldPath = Path.Combine(uploads, existingItem.Img);
                        if (System.IO.File.Exists(oldPath))
                            System.IO.File.Delete(oldPath);
                    }

                    imgFileName = Guid.NewGuid().ToString("N") + ext;
                    var filePath = Path.Combine(uploads, imgFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        files.CopyTo(stream);
                    }
                }

                // Создаем обновленный товар
                var updatUpdateem = new Items
                {
                    Id = id,
                    Name = name,
                    Description = descr ?? "",
                    Price = (int)price,
                    Category = new Categorys { Id = idCategory },
                    Img = imgFileName
                };

                // Вызываем метод обновления
                IAllItems.Update(updatUpdateem);

                TempData["Success"] = "Товар успешно обновлен!";
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ошибка при обновлении: " + ex.Message;
                return RedirectToAction("Update", new { id = id });
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var item = IAllItems.AllItems.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                TempData["Error"] = "Товар не найден!";
                return RedirectToAction("List");
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var item = IAllItems.AllItems.FirstOrDefault(i => i.Id == id);
                if (item == null)
                {
                    TempData["Error"] = "Товар не найден!";
                    return RedirectToAction("List");
                }

                // Удаляем файл изображения
                if (!string.IsNullOrEmpty(item.Img) && item.Img != "no-image.png")
                {
                    var uploads = Path.Combine(hostingEnvironment.WebRootPath, "img");
                    var filePath = Path.Combine(uploads, item.Img);
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                }

                IAllItems.Delete(id);
                TempData["Success"] = "Товар удален!";
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ошибка при удалении: " + ex.Message;
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public IActionResult Basket(int idItem = -1)
        {
            if (idItem == -1)
                return BadRequest();

            var item = IAllItems.AllItems.FirstOrDefault(x => x.Id == idItem);
            if (item == null)
                return NotFound();

            var existing = Startup.BasketItem.Find(x => x.Id == idItem);
            if (existing == null)
            {
                Startup.BasketItem.Add(new ItemsBasket { Id = idItem, Count = 1, Item = item });
            }
            else
            {
                existing.Count++;
            }

            return Json(Startup.BasketItem);
        }

        [HttpPost]
        public IActionResult BasketCount(int idItem, int count)
        {
            var existing = Startup.BasketItem.Find(x => x.Id == idItem);
            if (existing == null)
                return NotFound();

            if (count <= 0)
                Startup.BasketItem.Remove(existing);
            else
                existing.Count = count;

            return Json(Startup.BasketItem);
        }

        [HttpGet]
        public IActionResult BasketTotal()
        {
            int total = Startup.BasketItem.Sum(x => x.Count);
            return Json(total);
        }
    }
}