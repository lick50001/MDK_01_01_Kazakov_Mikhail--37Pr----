using MySql.Data.MySqlClient;
using Shop.Data.Common;
using Shop.Data.Interfaces;
using Shop.Data.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Shop.Data.Mocks
{
    public class MockItems : IItems
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
                       Category = _category.AllCategory.FirstOrDefault(x => x.Id == 0)
                    },
                    new Items(),
                    new Items(),
                    new Items(),
                    new Items()
                };
            }
        }

        public int Add(Items item)
        {
            MySqlConnection conn = Connection.MySqlOpen();
            Connection.MySqlQuery($"INSERT INTO `Shop`.`Items` (`Name`, `Description`, `Img`, `Price`, `Category`) VALUES ('{item.Name}', '{item.Description}', '{item.Img}', '{item.Price}', '{item.Category.Id}');", conn);
            conn.Close();

            int IdItem = -1;
            conn = Connection.MySqlOpen();
            MySqlDataReader myReader = Connection.MySqlQuery($"SELECT Id FROM Shop.Items WHERE Name = {item.Name} AND Description = {item.Description} AND Price = {item.Price} AND Category = {item.Category.Id};", conn);

            if (myReader.HasRows)
            {
                myReader.Read();
                IdItem = myReader.GetInt32(0);
            }

            conn.Close();
            return IdItem;
        }

        public void Update(Items item)
        {
            MySqlConnection conn = Connection.MySqlOpen();
            MySqlDataReader ItemsData = Connection.MySqlQuery($"UPDATE `Shop`.`Items` SET `Name` = '{item.Name}', `Description` = '{item.Description}', `Img` = '{item.Img}', `Price` = '{item.Price}', `Category` = '{item.Category.Id}' WHERE (`Id` = '{item.Id}');", conn);
            conn.Close();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
