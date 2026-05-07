using MySql.Data.MySqlClient;

namespace Shop.Data.Common
{
    public class Connection
    {
        readonly static string ConnData = "server=localhost;port=3306;database=Shop;uid=root;pwd=;";

        public static MySqlConnection MySqlOpen()
        {
            MySqlConnection conn = new MySqlConnection(ConnData);
            conn.Open();

            return conn;
        }

        public static MySqlDataReader MySqlQuery(string Query, MySqlConnection connection)
        {
            MySqlCommand cmd = new MySqlCommand(Query, connection);
            return cmd.ExecuteReader();
        }

        public static void MySqlClose(MySqlConnection connection) {
            connection.Close();
    }
}
