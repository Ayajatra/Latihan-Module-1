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
    public partial class Form4 : Form
    {
        public string ID;
        public Form4()
        {
            InitializeComponent();
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            ApplyChange();
            Close();
        }

        private void ApplyChange()
        {
            Connection.command = new SqlCommand("select ID from Offices" +
                $" where Title='{comboBoxOffice.Text}'", Connection.connection);

            int officeID = Convert.ToInt32(Connection.command.ExecuteScalar());

            int roleID = 2;
            if (radioButtonAdmin.Checked)
            {
                roleID = 1;
            }

            Connection.command = new SqlCommand("update Users" +
                " set Email=@Email, FirstName=@FirstName, LastName=@LastName, OfficeID=@OfficeID, RoleID=@RoleID" +
                " where ID=@ID", Connection.connection);

            Connection.command.Parameters.Add("@Email", SqlDbType.NVarChar, 150).Value = textBoxEmail.Text;
            Connection.command.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50).Value = textBoxFirstName.Text;
            Connection.command.Parameters.Add("@LastName", SqlDbType.NVarChar, 50).Value = textBoxLastName.Text;
            Connection.command.Parameters.Add("@OfficeID", SqlDbType.Int, -1).Value = officeID;
            Connection.command.Parameters.Add("@RoleID", SqlDbType.Int, -1).Value = roleID;
            Connection.command.Parameters.Add("@ID", SqlDbType.Int, -1).Value = ID;

            Connection.command.ExecuteNonQuery();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            comboBoxOffice.Items.Remove("All offices");
        }
    }
}
