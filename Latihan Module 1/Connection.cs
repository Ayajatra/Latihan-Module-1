using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Latihan_Module_1
{
    public static class Connection
    {
        // Connection String
        static string connectionString =
            "Server=localhost;" +
            "Database=Session1;" +
            "Integrated Security=True";

        public static void OpenConnection()
        {
            MessageBox.Show("Connecting to SQL Server");
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    MessageBox.Show("Connected");
                }
            }
            catch (SqlException error)
            {
                MessageBox.Show(error.ToString());
            }
        }
    }
}
