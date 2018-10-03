using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Latihan_Module_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Connection.OpenConnection();

            // Ambil data table [Activity] dari database
            using (var connection = new SqlConnection(Connection.connectionString))
            {
                connection.Open();
                var command = new SqlCommand("select * from [Activity]", connection);

                using (var reader = command.ExecuteReader())
                {
                    // TODO Buat datagridviewnya berfungsi
                    var activityTable = new DataTable();
                    activityTable.Load(reader);

                    dataGridView1.DataSource = reader;
                }
            }
        }
    }
}
