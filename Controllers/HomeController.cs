using Microsoft.AspNetCore.Mvc;

namespace Shop.Controllers
{
    public class HomeController
    {
        public RedirectResult Index()
        {
            return Redirect("/Items/List");
        }
    }
}
