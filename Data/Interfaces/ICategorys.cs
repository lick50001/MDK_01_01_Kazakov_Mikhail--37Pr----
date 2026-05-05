using Shop.Data.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Shop.Data.Interfaces
{
    public interface ICategorys
    {
        IEnumerable<Categorys> AllCategory {  get; }
    }
}
