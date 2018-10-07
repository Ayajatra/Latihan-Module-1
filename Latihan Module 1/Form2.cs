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

        public void ShowData()
        {
            string selectedOffice = comboBoxOffice.SelectedItem.ToString();

            // Supaya terpilih semua
            if (selectedOffice == "All offices")
            {
                selectedOffice = "' or 1='1";
            }

            Connection.adapter = new SqlDataAdapter("select U.ID, U.FirstName as Name, U.LastName as 'Last Name', datediff(year, U.Birthdate, convert(date, getdate())) as Age, R.Title as 'User Role', U.Email as 'Email Address', O.Title as Office, U.Active" +
                " from Users as U" +
                " inner join Roles as R on U.RoleID = R.ID" +
                " inner join Offices as O on U.OfficeID = O.ID" +
                $" where O.Title='{selectedOffice}'", Connection.connection);

            Connection.table.Clear();
            Connection.adapter.Fill(Connection.table);
            dataGridView1.DataSource = Connection.table;
            dataGridView1.Columns["ID"].Visible = false;
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

        private void buttonEnableLogin_Click(object sender, EventArgs e)
        {
            try
            {
                int cellId = Convert.ToInt32(dataGridView1.CurrentCell.OwningRow.Cells["ID"].Value);
                string sql;
                if (buttonEnableLogin.Text == "Enable Login")
                {
                    sql = "update Users" +
                        " set Active=1" +
                        $" where ID={cellId}";
                }
                else
                {
                    sql = "update Users" +
                        " set Active=0" +
                        $" where ID={cellId}";
                }
                Connection.command = new SqlCommand(sql, Connection.connection);
                Connection.command.ExecuteNonQuery();
                ShowData();
                buttonEnableLogin.Text = "Enable/Disable Login";
                buttonEnableLogin.Enabled = false;
            }
            catch { }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int active = Convert.ToInt32(dataGridView1.CurrentCell.OwningRow.Cells["Active"].Value);
            string role = Convert.ToString(dataGridView1.CurrentCell.OwningRow.Cells["User Role"].Value);
            if (active == 0)
            {
                if (role != "Administrator")
                {
                    buttonEnableLogin.Text = "Enable Login";
                    buttonEnableLogin.Enabled = true;
                }
                else
                {
                    buttonEnableLogin.Text = "Enable/Disable Login";
                    buttonEnableLogin.Enabled = false;
                }
            }
            else
            {
                if (role != "Administrator")
                {
                    buttonEnableLogin.Text = "Disable Login";
                    buttonEnableLogin.Enabled = true;
                }
                else
                {
                    buttonEnableLogin.Text = "Enable/Disable Login";
                    buttonEnableLogin.Enabled = false;
                }
            }

            buttonChangeRole.Enabled = true;
        }

        private void buttonAddUser_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.buttonSave.Click += ButtonSave_Click;
            form3.ShowDialog();
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            ShowData();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonChangeRole_Click(object sender, EventArgs e)
        {
            var form4 = new Form4();
            form4.buttonApply.Click += ButtonApply_Click;

            form4.ID = dataGridView1.CurrentCell.OwningRow.Cells["ID"].Value.ToString();
            form4.textBoxEmail.Text = dataGridView1.CurrentCell.OwningRow.Cells["Email Address"].Value.ToString();
            form4.textBoxFirstName.Text = dataGridView1.CurrentCell.OwningRow.Cells["Name"].Value.ToString();
            form4.textBoxLastName.Text = dataGridView1.CurrentCell.OwningRow.Cells["Last Name"].Value.ToString();

            // Copy form2.Combobox ke form4.Combobox
            // Supaya bisa cari nama office yang sesuai
            form4.comboBoxOffice.Items.AddRange(comboBoxOffice.Items.Cast<object>().ToArray());            

            // Munculkan nama office sesuai dengan datagridview
            string office = dataGridView1.CurrentCell.OwningRow.Cells["Office"].Value.ToString();
            int officeIndex = form4.comboBoxOffice.FindStringExact(office);
            form4.comboBoxOffice.SelectedIndex = officeIndex;

            string userRole = dataGridView1.CurrentCell.OwningRow.Cells["User Role"].Value.ToString();
            if (userRole == "User")
            {
                form4.radioButtonUser.Checked = true;
            }
            else if (userRole == "Administrator")
            {
                form4.radioButtonAdmin.Checked = true;
            }

            form4.ShowDialog();
            buttonChangeRole.Enabled = false;
        }

        private void ButtonApply_Click(object sender, EventArgs e)
        {
            ShowData();
        }
    }
}
