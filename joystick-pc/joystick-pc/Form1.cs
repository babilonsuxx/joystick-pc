using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace joystick_pc
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //получаем текущий ip

// Получение имени компьютера.
String host = System.Net.Dns.GetHostName();
// Получение ip-адреса.
System.Net.IPAddress ip = System.Net.Dns.GetHostByName(host).AddressList[0];
// Показ адреса в label'е.
labelIp.Text = ip.ToString();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
            btnStart.Enabled = true;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = true;
            btnStart.Enabled = false;
        }
        // вводим только числа
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Convert.ToChar(8))
                {
                     e.Handled = true;
                }
        
        }
    }
}
