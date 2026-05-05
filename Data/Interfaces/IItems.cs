using Shop.Data.Models;
using System.Collections;

namespace Shop.Data.Interfaces
{
    public interface IItems
    {
        public IEnumerable<Items> AllItems { get; }
    }
}
