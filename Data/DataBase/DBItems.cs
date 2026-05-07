using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using Shop.Data.Common;
using Shop.Data.Interfaces;
using Shop.Data.Models;

namespace Shop.Data.DataBase
{
    public class DBItems : IItems
    {
        public IEnumerable<Categorys> Categorys = (IEnumerable<Categorys>)new DBCategory().AllCategory;
        private readonly ILogger<DBItems> _logger;
        public DBItems(ILogger<DBItems> logger) => _logger = logger;

        public IEnumerable<Items> AllItems
        {
            get
            {
                List<Items> items = new List<Items>();
                MySqlConnection conn = Connection.MySqlOpen();
                MySqlDataReader ItemsData = Connection.MySqlQuery("SELECT * FROM Shop.items ORDER BY Name;", conn);
                while (ItemsData.Read())
                {
                    items.Add(new Items()
                    {
                        Id = ItemsData.IsDBNull(0) ? -1 : ItemsData.GetInt32(0),
                        Name = ItemsData.IsDBNull(1) ? "" : ItemsData.GetString(1),
                        Description = ItemsData.IsDBNull(2) ? "" : ItemsData.GetString(2),
                        Img = ItemsData.IsDBNull(3) ? "" : ItemsData.GetString(3),
                        Price = ItemsData.IsDBNull(4) ? -1 : ItemsData.GetInt32(4),
                        Category = ItemsData.IsDBNull(5) ? null : Categorys.Where(x => x.Id == ItemsData.GetInt32(5)).First()
                    });
                }
                return items;
            }
        }

        public int Add(Items item)
        {
            MySqlConnection conn = Connection.MySqlOpen();
            Connection.MySqlQuery($"INSERT INTO `Shop`.`Items` (`Name`, `Description`, `Img`, `Price`, `Category`) VALUES ('{item.Name}', '{item.Description}', '{item.Img}', '{item.Price}', '{item.Category.Id}');", conn);
            conn.Close();

            int IdItem = -1;
            conn = Connection.MySqlOpen();
            MySqlDataReader myReader = Connection.MySqlQuery($"SELECT Id FROM Shop.Items WHERE Name = '{item.Name}' AND Description = '{item.Description}' AND Price = '{item.Price}' AND Category = '{item.Category.Id}';", conn);

            if (myReader.HasRows)
            {
                myReader.Read();
                IdItem = myReader.GetInt32(0);
            }

            conn.Close();
            return IdItem;
        }


        public void Delete(int id)
        {
            MySqlConnection conn = Connection.MySqlOpen();
            MySqlDataReader ItemsData = Connection.MySqlQuery($"DELETE FROM `Shop`.`Items` WHERE (`Id` = '{id}');", conn);
            conn.Close();
        }

        public void Update(Items item)
        {
            const string ConnData = "server=localhost;port=3306;database=Shop;uid=root;pwd=;";
            using var conn = new MySqlConnection(ConnData);
            conn.Open();

            using var cmd = new MySqlCommand(@"
            UPDATE items 
            SET Name = @name, Description = @descr, Img = @img, Price = @price, Category = @catId 
            WHERE Id = @id", conn);

            cmd.Parameters.AddWithValue("@id", item.Id);
            cmd.Parameters.AddWithValue("@name", item.Name ?? "");
            cmd.Parameters.AddWithValue("@descr", item.Description ?? "");
            cmd.Parameters.AddWithValue("@img", item.Img ?? "");
            cmd.Parameters.AddWithValue("@price", item.Price);
            cmd.Parameters.AddWithValue("@catId", item.Category?.Id ?? 0);

            cmd.ExecuteNonQuery();
        }
    }
}
