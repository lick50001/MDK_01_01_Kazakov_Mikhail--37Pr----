using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Shop.Data.Common;
using Shop.Data.Interfaces;
using Shop.Data.Models;

namespace Shop.Data.DataBase
{
    public class DBCategory
    {
        public IEnumerable<ICategorys> AllCategorys
        {
            get
            {
                List<ICategorys> categorys = new List<ICategorys>();
                MySqlConnection conn = Connection.MySqlOpen();
                MySqlDataReader CategorysData = Connection.MySqlQuery("SELECT * FROM Shop.Categorys ORDER BY 'Name';", conn);
                while (CategorysData.Read())
                {
                    categorys.Add(new Categorys()
                    {
                        Id = CategorysData.IsDBNull(0) ? -1 : CategorysData.GetInt32(0),
                        Name = CategorysData.IsDBNull(1) ? null : CategorysData.GetString(1),
                        Description = CategorysData.IsDBNull(2) ? null : CategorysData.GetString(2),
                    });
                }
                return categorys;
            }
        }
    }
}