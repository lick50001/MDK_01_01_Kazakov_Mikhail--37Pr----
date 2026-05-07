using Microsoft.AspNetCore.Mvc;
using Shop.Data.Interfaces;
using Shop.Data.Models;
using Shop.Data.ViewModels;

namespace Shop.Controllers
{
    public class ItemsController : Microsoft.AspNetCore.Mvc.Controller
    {
        public IItems IAllItems;
        private ICategorys IAllCategorys;
        VMItems VMItems = new VMItems();
        public ItemsController(IItems IAllItems, ICategorys IAllCategorys)
        {
            this.IAllItems = IAllItems;
            this.IAllCategorys = IAllCategorys;
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
    }
}

