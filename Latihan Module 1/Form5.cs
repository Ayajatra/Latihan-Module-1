using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Latihan_Module_1
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            InitTime();
        }

        string year;
        string month;
        string date;
        private void LoadDate()
        {
            var dateTime = DateTime.Now;
            year = dateTime.Year.ToString();
            month = dateTime.Month.ToString().PadLeft(2, '0');
            date = dateTime.Day.ToString().PadLeft(2, '0');
        }

        // Set starting time
        DateTime startingDateTime = DateTime.Now;
        private void InitTime()
        {
            // 1 detik = 1000 milisecond
            var timer = new Timer { Interval = 1000 };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var nowDateTime = DateTime.Now;
            string differenceHour = nowDateTime.Subtract(startingDateTime).Hours.ToString().PadLeft(2, '0');
            string differenceMinute = nowDateTime.Subtract(startingDateTime).Minutes.ToString().PadLeft(2, '0');
            string differenceSecond = nowDateTime.Subtract(startingDateTime).Seconds.ToString().PadLeft(2, '0');

            // Set difference time to the label
            labelTime.Text = $"{differenceHour} : {differenceMinute} : {differenceSecond}";
            LoadDate();
        }

        private void Form5_FormClosed(object sender, FormClosedEventArgs e)
        {
            var form1 = Application.OpenForms.Cast<Form>().FirstOrDefault(x => x is Form1);
            form1.Close();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
