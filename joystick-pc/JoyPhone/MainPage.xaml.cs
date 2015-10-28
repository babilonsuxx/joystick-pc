

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//тут мое 
using Windows.Networking.Sockets;
using Windows.Networking;
using Windows.Storage.Streams;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace JoyPhone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        StreamSocket clientSocket = new StreamSocket();
        
        
        

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private async void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            HostName host = new HostName(textBoxIP.Text);
            string port = textBoxPort.Text;
            if (connected)
            {
                StatusText.Text = "уже подключен";
                return;
            }

            try
            {
                
                StatusText.Text = "попытка подключения ...";

                
                // Try to connect to the 
                await clientSocket.ConnectAsync(host, port);
                connected = true;
                StatusText.Text = "подключение установлено" + Environment.NewLine;

            }
            catch (Exception exception)
            {
                // If this is an unknown status, 
                // it means that the error is fatal and retry will likely fail.
                if (SocketError.GetStatus(exception.HResult) == SocketErrorStatus.Unknown)
                {
                    throw;
                }

                StatusText.Text = "не удалось установить подключение: "; //+ exception.Message;
                // Could retry the connection, but for this simple example
                // just close the socket.

                closing = true;
                // the Close method is mapped to the C# Dispose
                clientSocket.Dispose();
                clientSocket = null;

            }
        }

        public bool connected { get; set; }

        public bool closing { get; set; }


 

        // функция для отправки состояния кнопки на сервер
        private async void sendkey(string key)
        {
            if (!connected)
            {
                StatusText.Text = "необходимо подключение";
                return;
            }

            //Int32 len = 0; // Gets the UTF-8 string length.

            try
            {

                StatusText.Text = "попытка отправки данных ...";

                // add a newline to the text to send
               
                DataWriter writer = new DataWriter(clientSocket.OutputStream);
                //Int32 len = writer.MeasureString(sendData); // Gets the UTF-8 string length.
                writer.WriteString(key);
                // Call StoreAsync method to store the data to a backing stream
                await writer.StoreAsync();

                StatusText.Text = "отправка успешна" + Environment.NewLine;

                // detach the stream and close it
                //попробуем не закрывать сокет
                writer.DetachStream();
                writer.Dispose();

            }
            catch (Exception exception)
            {
                // If this is an unknown status, 
                // it means that the error is fatal and retry will likely fail.
                if (SocketError.GetStatus(exception.HResult) == SocketErrorStatus.Unknown)
                {
                    throw;
                }

                StatusText.Text = "Не удалось оправить данные ";// +exception.Message;
                // Could retry the connection, but for this simple example
                // just close the socket.

                closing = true;
                clientSocket.Dispose();
                clientSocket = null;
                connected = false;

            }

           
        }

       
        //посылаем значения кнопок
        //вверх
        private void gotFocusUp(object sender, RoutedEventArgs e)
        {
            sendkey("w");
        }

        private void lostFocusUp(object sender, RoutedEventArgs e)
        {
            sendkey("w1");
            btnConnect.Focus(FocusState.Programmatic);
        }
        private void lostFocusUp(object sender, PointerRoutedEventArgs e)
        {
            sendkey("w1");
            btnConnect.Focus(FocusState.Programmatic);
        }
       //вниз
        private void gotFocusDown(object sender, RoutedEventArgs e)
        {
            sendkey("s");
        }

        private void lostFocusDown(object sender, RoutedEventArgs e)
        {
            sendkey("s1");
            btnConnect.Focus(FocusState.Programmatic);
        }
        private void lostFocusDown(object sender, PointerRoutedEventArgs e)
        {
            sendkey("s1");
            btnConnect.Focus(FocusState.Programmatic);
        }
        //лево

        private void gotFocusLeft(object sender, RoutedEventArgs e)
        {
            sendkey("a");
        }
        private void lostFocusLeft(object sender, RoutedEventArgs e)
        {
            sendkey("a1");
            btnConnect.Focus(FocusState.Programmatic);
        }

        private void lostFocusLeft(object sender, PointerRoutedEventArgs e)
        {
            sendkey("a1");
            btnConnect.Focus(FocusState.Programmatic);
        }

        //право
        private void gotFocusRight(object sender, RoutedEventArgs e)
        {
            sendkey("d");
        }

        private void lostFocusRight(object sender, RoutedEventArgs e)
        {
            sendkey("d1");
            btnConnect.Focus(FocusState.Programmatic);
        }
        private void lostFocusRight(object sender, PointerRoutedEventArgs e)
        {
            sendkey("d1");
            btnConnect.Focus(FocusState.Programmatic);
        }

        // select
        private void gotFocusSelect(object sender, RoutedEventArgs e)
        {
            sendkey("z");
        }

        private void lostFocusSelect(object sender, RoutedEventArgs e)
        {
            sendkey("z1");
            btnConnect.Focus(FocusState.Programmatic);
        }
        private void lostFocusSelect(object sender, PointerRoutedEventArgs e)
        {
            sendkey("z1");
            btnConnect.Focus(FocusState.Programmatic);
        }
        // start
        private void gotFocusStart(object sender, RoutedEventArgs e)
        {
            sendkey("x");
        }
        private void lostFocusStart(object sender, RoutedEventArgs e)
        {
            sendkey("x1");
            btnConnect.Focus(FocusState.Programmatic);
        }

        private void lostFocusStart(object sender, PointerRoutedEventArgs e)
        {
            sendkey("x1");
            btnConnect.Focus(FocusState.Programmatic);
        }
        // btnOne
        private void gotFocusOne(object sender, RoutedEventArgs e)
        {
            sendkey("q");
        }

        private void lostFocusOne(object sender, RoutedEventArgs e)
        {
            sendkey("q1");
            btnConnect.Focus(FocusState.Programmatic);
        }
        private void lostFocusOne(object sender, PointerRoutedEventArgs e)
        {
            sendkey("q1");
            btnConnect.Focus(FocusState.Programmatic);
        }
        // btnTwo
        private void gotFocusTwo(object sender, RoutedEventArgs e)
        {
            sendkey("e");
        }

        private void lostFocusTwo(object sender, RoutedEventArgs e)
        {
            sendkey("e1");
            btnConnect.Focus(FocusState.Programmatic);
        }
        private void lostFocusTwo(object sender, PointerRoutedEventArgs e)
        {
            sendkey("e1");
            btnConnect.Focus(FocusState.Programmatic);
        }

        private void gotFocusUnic(object sender, RoutedEventArgs e)
        {

            sendkey(sender.ToString());
            //Frame.Navigate(typeof(ChangeBeh), itemId);

        }   
    }
}
