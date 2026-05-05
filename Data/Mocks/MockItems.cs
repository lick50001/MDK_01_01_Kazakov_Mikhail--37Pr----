using Shop.Data.Interfaces;
using Shop.Data.Models;
using System.Collections;
using System.Collections.Generic;

namespace Shop.Data.Mocks
{
    public class MockItems
    {
        public ICategorys _category = new MockCategorys();
        public IEnumerable<Items> AllItems
        {
            get
            {
                return new List<Items>()
                {
                    new Items()
                    {
                        Id = 0,
                        Name = "DEXP M5-70",
                        Description = "Благодаря черному корпусу с лаконичным дизайном",
                        Price = 3699,
                        Category = _category.AllCategory.Where(x => x.Id == 0).First()
                    },
                    new Items(),
                    new Items(),
                    new Items(),
                    new Items()
                };
            }
        }
    }
}
