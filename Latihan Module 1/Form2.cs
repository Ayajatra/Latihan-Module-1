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
            ShowList();
            ShowData();

            // Event ini ditambahkan disini supaya ShowData() tidak di panggil bersamaan
            // (yang menyebabkan crash)
            comboBoxOffice.SelectedIndexChanged += ComboBoxOffice_SelectedIndexChanged;
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            var form1 = Application.OpenForms.Cast<Form>().FirstOrDefault(x => x is Form1);
            form1.Close();
        }

        private void ShowList()
        {
            Connection.command = new SqlCommand("select Title from Offices", Connection.connection);
            Connection.reader = Connection.command.ExecuteReader();
            comboBoxOffice.Items.Add("All offices");
            while (Connection.reader.Read())
            {
                comboBoxOffice.Items.Add(Connection.reader[0]);
            }

            comboBoxOffice.SelectedIndex = 0;
            Connection.reader.Close();
        }

        private void ComboBoxOffice_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowData();
        }

        private void ShowData()
        {
            string selectedOffice = comboBoxOffice.SelectedItem.ToString();

            // Supaya terpilih semua
            if (selectedOffice == "All offices")
            {
                selectedOffice = "' or 1='1";
            }

            Connection.adapter = new SqlDataAdapter("select U.FirstName as Name, U.LastName as 'Last Name', datediff(year, U.Birthdate, convert(date, getdate())) as Age, R.Title as 'User Role', U.Email as 'Email Address', O.Title as Office, U.Active" +
                " from Users as U" +
                " inner join Roles as R on U.RoleID = R.ID" +
                " inner join Offices as O on U.OfficeID = O.ID" +
                $" where O.Title='{selectedOffice}'", Connection.connection);

            Connection.table.Clear();
            Connection.adapter.Fill(Connection.table);
            dataGridView1.DataSource = Connection.table;
            dataGridView1.Columns["Active"].Visible = false;
            ColorData();
        }

        private void ColorData()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (Convert.ToInt32(row.Cells["Active"].Value) == 0)
                {
                    row.DefaultCellStyle.BackColor = Color.IndianRed;
                }

                if (Convert.ToString(row.Cells["User Role"].Value) == "Administrator")
                {
                    row.DefaultCellStyle.BackColor = Color.LightGreen;
                }
            }
        }
    }
}
