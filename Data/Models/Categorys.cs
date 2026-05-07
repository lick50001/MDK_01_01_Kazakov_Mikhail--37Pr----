using System.Collections.Generic;
using Shop.Data.Interfaces;

namespace Shop.Data.Models
{
    public class Categorys : ICategorys
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Items> Items { get; set; }
    }
}
