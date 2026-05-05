using Shop.Data.Models;
using System.Collections;

namespace Shop.Data.Mocks
{
    public class MockCaregorys
    {
        public IEnumerable<Categorys> AllCategorys
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
