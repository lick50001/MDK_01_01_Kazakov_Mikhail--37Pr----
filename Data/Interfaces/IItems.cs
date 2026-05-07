using Shop.Data.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Shop.Data.Interfaces
{
    public interface IItems
    {
        public IEnumerable<Items> AllItems { get; }
        public int Add(Items Item);
        public void Delete(int id);
        public void Update(int Id);
    }
}
