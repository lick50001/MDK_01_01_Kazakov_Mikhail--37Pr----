using Shop.Data.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Shop.Data.Interfaces
{
    public interface ICategorys
    {
        int Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        //IEnumerable<Categorys> AllCategory {  get; }
    }
}
