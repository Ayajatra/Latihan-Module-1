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
        int loginCount = 0;
        Timer timer = new Timer()
        {
            Interval = 1000,
            Enabled = true
        };

        int coolDown = 10;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Connection.OpenConnection();
            timer.Tick += Timer_Tick;
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            LoginCheck();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LoginCheck()
        {
            if (loginCount != 3)
            {
                // Untuk men-sanitasi input user

                Connection.command = new SqlCommand("select count(*) from Users where Email=@Email and Password=@Password", Connection.connection);
                Connection.command.Parameters.Add("@Email", SqlDbType.NVarChar, 150).Value = textBoxUsername.Text;
                Connection.command.Parameters.Add("@Password", SqlDbType.NVarChar, 50).Value = textBoxPassword.Text;

                // Hitung hasil dari query

                int numRows = Convert.ToInt32(Connection.command.ExecuteScalar());
                if (numRows == 1)
                {
                    // Check apakah di disable oleh administrator atau tidak

                    Connection.command = new SqlCommand("select cast(Active as int) from Users where Email=@Email and Password=@Password", Connection.connection);
                    Connection.command.Parameters.Add("@Email", SqlDbType.NVarChar, 150).Value = textBoxUsername.Text;
                    Connection.command.Parameters.Add("@Password", SqlDbType.NVarChar, 50).Value = textBoxPassword.Text;

                    bool active = Convert.ToBoolean(Connection.command.ExecuteScalar());
                    if (active)
                    {
                        var form2 = new Form2();
                        form2.Show();
                        Hide();
                    }
                    else
                    {
                        MessageBox.Show("Anda tidak diizinkan untuk login oleh administrator");
                    }
                }
                else
                {
                    loginCount++;
                    MessageBox.Show("Salah Username atau Password");
                }
            }
            else
            {
                loginCount = 0;
                buttonLogin.Enabled = false;
                labelTimer.Visible = true;
                timer.Start();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            labelTimer.Text = $"Please wait {coolDown}s before you can login again";
            if (coolDown != 0)
            {
                coolDown--;
            }
            else
            {
                timer.Stop();
                coolDown = 10;
                buttonLogin.Enabled = true;
                labelTimer.Visible = false;
            }
        }
    }
}
