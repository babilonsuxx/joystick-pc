using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace joystick_pc
{
    public partial class ServerForm : Form
    {
        private Socket _serverSocket, _clientSocket;
        private byte[] _buffer;

        public ServerForm()
        {
            //string path = @"F:\joystick-project\joystick-pc\joystick-pc\client\bin\Debug\client.exe";
            //Process.Start(path);
            InitializeComponent();

            StartServer();

        }

        private void StartServer()
        {
            try
            {
                _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _serverSocket.Bind(new IPEndPoint(IPAddress.Any, 3333));
                _serverSocket.Listen(0);
                _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AcceptCallback(IAsyncResult AR)
        {
            try
            {
                _clientSocket = _serverSocket.EndAccept(AR);
                _buffer = new byte[_clientSocket.ReceiveBufferSize];
                _clientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReceiveCallback(IAsyncResult AR)
        {
            try
            {
                int received = _clientSocket.EndReceive(AR);
                Array.Resize(ref _buffer, received);
                string text = Encoding.ASCII.GetString(_buffer);
                AppendToTextBox(text);
                Array.Resize(ref _buffer, _clientSocket.ReceiveBufferSize);
                _clientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private  void AppendToTextBox(string text)
        {
            MethodInvoker invoker = new MethodInvoker(delegate
                {
                    string text_before = text;
                    string exitW = text;
                    
                    if (text == "w")
                    {
                        KeyDown(Keys.W);
                        textBox.Text += text + " ";

                        
                    }
                    if (text == "s")
                    {
                        KeyDown(Keys.S);
                        textBox.Text += text + " ";
                       
                    }
                    if (text == "a")
                    {
                        KeyUp(Keys.D);
                        KeyDown(Keys.A);
                        textBox.Text += text + " ";

                    }
                    if (text == "d")
                    {
                        KeyUp(Keys.A);
                        KeyDown(Keys.D);
                        textBox.Text += text + " ";

                    }
                    if (text == "z")
                    {
                        KeyDown(Keys.Z);
                        textBox.Text += text + " ";
                    }
                    if (text == "x")
                    {
                        KeyDown(Keys.X);
                        textBox.Text += text + " ";
                    }
                    
                    if (text == "q")
                    {
                        KeyDown(Keys.Q);
                        textBox.Text += text + " ";
                    
                    if (text == "e")
                    {
                        KeyDown(Keys.E);
                        textBox.Text += text + " ";
                       
                    }

                    

                    //отжимаем
                    if (text == "w1"  )
                    {
                        KeyUp(Keys.W);
                        textBox.Text += text + " ";
                    }
                    if (text == "s1" )
                    {
                        KeyUp(Keys.S);
                        textBox.Text += text + " ";
                    }
                    if (text == "a1" )
                    {
                        KeyUp(Keys.A);
                        textBox.Text += text + " ";
                    }
                    if (text == "d1" )
                    {
                        KeyUp(Keys.D);
                        textBox.Text += text + " ";
                    }
                    if (text == "z1" )
                    {
                        KeyUp(Keys.Z);
                        textBox.Text += text + " ";
                    }
                    if (text == "x1" )
                    {
                        KeyUp(Keys.X);
                        textBox.Text += text + " ";
                    }

                    if (text == "q1" )
                    {
                        KeyUp(Keys.Q);
                        textBox.Text += text + " ";
                    }
                    if (text == "e1" )
                    {
                        KeyUp(Keys.E);
                        textBox.Text += text + " ";
                    }
                    
                        

                    
                });
            this.Invoke(invoker);
        }

        //работа с клавой
       [ DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        private const int KEYEVENTF_EXTENDEDKEY = 1;
        private const int KEYEVENTF_KEYUP = 2;

        public static void KeyDown(Keys vKey)
        {
            keybd_event((byte)vKey, 0, KEYEVENTF_EXTENDEDKEY, 0);
        }

        public static void KeyUp(Keys vKey)
        {
                keybd_event((byte)vKey, 0, KEYEVENTF_KEYUP, 0);
            
        }

        

        
    
    
    }

    

}
