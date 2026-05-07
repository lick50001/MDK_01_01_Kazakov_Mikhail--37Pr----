using Shop.Data.Models;
using System.Collections.Generic;

namespace Shop.Data.ViewModels
{
    public class VMItems
    {
        public IEnumerable<Items> Items { get; set; }
        public IEnumerable<Categorys> Categorys { get; set; }
        public int SelectCategory { get; set; }
    }
}