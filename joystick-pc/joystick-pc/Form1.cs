﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Net;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;

namespace joystick_pc
{
    public partial class Form1 : Form
    {
       
        
        public Form1()
        {
            InitializeComponent();


            // Получение имени компьютера.
            String host = System.Net.Dns.GetHostName();
            // Получение ip-адреса.
            System.Net.IPAddress ip = System.Net.Dns.GetHostByName(host).AddressList[0];
            // Показ адреса в label'е.
            labelIp.Text = ip.ToString();
        }

       

        private void btnStart_Click(object sender, EventArgs e)
        {

            if (btnStart.Text == "START")
            {
                //меняем кнопку и цвет
                btnStart.Text = "STOP";
                btnStart.ForeColor = System.Drawing.Color.Red;
                //номер порта
                int portNum = Convert.ToInt32(textBox1.Text);
                // открываем сокет
                String host = System.Net.Dns.GetHostName();
                IPAddress ipAddr = System.Net.Dns.GetHostByName(host).AddressList[0];
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, portNum);
                Socket sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);



            }
            else
            {
                //меняем кнопку и цвет
                btnStart.Text = "START";
                btnStart.ForeColor = System.Drawing.Color.Green;
                //останавливаем 


            }
            
        }
        // вводим только числа
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Convert.ToChar(8))
                {
                     e.Handled = true;
                }
        
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

       
    }
}
