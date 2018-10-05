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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            ShowData();
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            var form1 = Application.OpenForms.Cast<Form>().FirstOrDefault(x => x is Form1);
            form1.Close();
        }

        private void ShowData()
        {
            Connection.adapter = new SqlDataAdapter("select U.FirstName as name, U.LastName as 'Last Name', datediff(year, U.Birthdate, convert(date, getdate())) as Age, R.Title as 'User Role', U.Email as 'Email Address', O.Title as Office" +
                " from Users as U" +
                " inner join Roles as R on U.RoleID = R.ID" +
                " inner join Offices as O on U.OfficeID = O.ID", Connection.connection);

            Connection.adapter.Fill(Connection.table);
            dataGridView1.DataSource = Connection.table;
        }
    }
}
