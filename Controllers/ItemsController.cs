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
        private readonly IHostingEnvironment hostingEnvironment;
        VMItems VMItems = new VMItems();
        public ItemsController(IItems IAllItems, ICategorys IAllCategorys, IHostingEnvironment environment)
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
        public RedirectResult Add(string name, string descr, IFormFile files, float price, int idCategory)
        {
            if(files != null)
            {
                var uplaoads = Path.Combine(hostingEnvironment.WebRootPath, "img");
                var filePath = Path.Combine(uplaoads, files.FileName);
                files.CopyTo(new FileStream(filePath, FileMode.Create));
            }

            Items newItm = new Items();
            newItm.Name = name;
            newItm.Description = descr;
            newItm.Img = files.FileName;
            newItm.Price = Convert.ToInt32(price);
            newItm.Category = new Categorys() { Id = idCategory };
            int id = IAllItems.Add(newItm);
            return Redirect("/Items/Update?id=" + id);
        }
    }
}

