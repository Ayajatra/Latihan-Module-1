using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;
using System.Data;

namespace Latihan_Module_1
{
    public static class Connection
    {
        // Ambil connectionString dari App.config
        public static readonly string connectionString = ConfigurationManager.ConnectionStrings["Session1"].ConnectionString;
        public static SqlConnection connection = new SqlConnection(connectionString);
        public static SqlCommand command;
        public static SqlDataAdapter adapter;
        public static DataSet dataSet = new DataSet();

        public static void OpenConnection()
        {
            // MessageBox.Show("Connecting to SQL Server");
            try
            {
                connection.Open();
                // MessageBox.Show("Connected");
            }
            catch (SqlException error)
            {
                MessageBox.Show(error.ToString());
            }
        }
    }
}
