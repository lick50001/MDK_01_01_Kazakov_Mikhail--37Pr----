using Shop.Data.Interfaces;
using Shop.Data.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Shop.Data.Mocks
{
    public class MockCategorys : ICategorys
    {
        public IEnumerable<Categorys> AllCategory
        {
            get
            {
                return new List<Categorys>
                {
                    new Categorys()
                    {
                        Id = 0,
                        Name = "Микроволновая печь",
                        Description = "Микроволновая"
                    },
                    new Categorys()
                    {
                        Id = 1,
                        Name = "Мультиварка",
                        Description = "Мультиварка ваау"
                    }
                };
            }            
        }
    }
}
