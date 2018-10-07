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
using System.Text.RegularExpressions;

namespace Latihan_Module_1
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            ShowList();
        }

        private void ShowList()
        {
            Connection.command = new SqlCommand("select Title from Offices", Connection.connection);
            Connection.reader = Connection.command.ExecuteReader();
            while (Connection.reader.Read())
            {
                comboBoxOffice.Items.Add(Connection.reader[0]);
            }

            comboBoxOffice.SelectedIndex = 0;
            Connection.reader.Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (ValidateEmail())
            {
                Connection.command = new SqlCommand("select top 1 ID from Users order by ID desc", Connection.connection);
                int lastId = Convert.ToInt32(Connection.command.ExecuteScalar());

                var sql = $"select ID from Offices where Title='{comboBoxOffice.Text}'";
                Connection.command = new SqlCommand(sql, Connection.connection);
                int officeId = Convert.ToInt32(Connection.command.ExecuteScalar());

                Connection.command = new SqlCommand("insert into Users" +
                    " (ID, RoleID, Email, Password, FirstName, LastName, OfficeID, Birthdate, Active)" +
                    " values(@ID, @RoleID, @Email, @Password, @FirstName, @LastName, @OfficeID, @Birthdate, @Active)", Connection.connection);

                Connection.command.Parameters.Add("@ID", SqlDbType.Int, -1).Value = ++lastId;
                Connection.command.Parameters.Add("@RoleID", SqlDbType.Int, -1).Value = 2;
                Connection.command.Parameters.Add("@Email", SqlDbType.NVarChar, 150).Value = textBoxEmailAddress.Text;
                Connection.command.Parameters.Add("@Password", SqlDbType.NVarChar, 50).Value = textBoxPassword.Text;
                Connection.command.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50).Value = textBoxFirstName.Text;
                Connection.command.Parameters.Add("@LastName", SqlDbType.NVarChar, 50).Value = textBoxLastName.Text;
                Connection.command.Parameters.Add("@OfficeID", SqlDbType.Int, -1).Value = officeId;
                Connection.command.Parameters.Add("@Birthdate", SqlDbType.Date, -1).Value = dateTimePickerBirthDate.Value;
                Connection.command.Parameters.Add("@Active", SqlDbType.Bit, -1).Value = 1;

                Connection.command.ExecuteNonQuery();
                ClearField();
            }
            else
            {
                MessageBox.Show("Harap memasukkan format email yang benar (aa@aa.aa)");
            }
        }

        private bool ValidateEmail()
        {
            var email = textBoxEmailAddress.Text;

            // Hilangkan whitespace
            Regex.Replace(email, @"\s+", "");
            if (email != string.Empty)
            {
                if (email.Contains("@"))
                {
                    string[] array1 = email.Split('@');
                    if (array1.Length == 2 && array1[0] != string.Empty && array1[1] != string.Empty)
                    {
                        string[] array2 = array1[1].Split('.');
                        if (array2.Length == 2 && array2[0] != string.Empty && array2[1] != string.Empty)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private void ClearField()
        {
            foreach (Control control in Controls)
            {
                if (control is TextBox)
                {
                    control.Text = String.Empty;
                }
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
