using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shop.Data.DataBase;
using Shop.Data.Interfaces;
using Shop.Data.Models;

namespace Shop
{
    public class Startup
    {
        public static List<ItemsBasket> BasketItem = new List<ItemsBasket>();
    }
}