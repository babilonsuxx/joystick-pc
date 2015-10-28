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
using Windows.Networking;
using Windows.Networking.Sockets;

using System.Diagnostics;

using Windows.Storage.Streams;





// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace joystick_phone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
    private StreamSocket _clientSocket;
    private HostName serverHost;
    private string serverHostnameString;
    private string serverPort;
    private bool connected = false;
    private bool closing = false;
    

        public MainPage()
        {
            this.InitializeComponent();
            _clientSocket = new StreamSocket();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

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

        private async void Connect_Click(object sender, RoutedEventArgs e)
        {
            if (connected)
            {
                StatusText.Text = "Already connected";
                return;
            }

            try
            {
                OutputView.Text = "";
                StatusText.Text = "Trying to connect ...";

                serverHost = new HostName(ServerHostname.Text);
                // Try to connect to the 
                await _clientSocket.ConnectAsync(serverHost, ServerPort.Text);
                connected = true;
                StatusText.Text = "Connection established" + Environment.NewLine;

            }
            catch (Exception exception)
            {
                // If this is an unknown status, 
                // it means that the error is fatal and retry will likely fail.
                if (SocketError.GetStatus(exception.HResult) == SocketErrorStatus.Unknown)
                {
                    throw;
                }

                StatusText.Text = "Connect failed with error: " + exception.Message;
                // Could retry the connection, but for this simple example
                // just close the socket.

                closing = true;
                // the Close method is mapped to the C# Dispose
                _clientSocket.Dispose();
                _clientSocket = null;

            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!connected)
            {
                StatusText.Text = "Must be connected to send!";
                return;
            }

           // Int32 len = 0; // Gets the UTF-8 string length.

            try
            {
                OutputView.Text = "";
                StatusText.Text = "Trying to send data ...";

                // add a newline to the text to send
                string sendData = SendText.Text + Environment.NewLine;
                DataWriter writer = new DataWriter(_clientSocket.OutputStream);
               UInt32 len = writer.MeasureString(sendData); // Gets the UTF-8 string length.
               writer.WriteString("123");
               //byte[] buffer = Encoding.ASCII.GetBytes(textBox.Text);
               //_clientSocket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), null);

                // Call StoreAsync method to store the data to a backing stream
                await writer.StoreAsync();

                StatusText.Text = "Data was sent" + Environment.NewLine;

                // detach the stream and close it
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

                StatusText.Text = "Send data or receive failed with error: " + exception.Message;
                // Could retry the connection, but for this simple example
                // just close the socket.

                closing = true;
                _clientSocket.Dispose();
                _clientSocket = null;
                connected = false;

            }

            // Now try to receive data from server
            try
            {
                OutputView.Text = "";
                StatusText.Text = "Trying to receive data ...";

                DataReader reader = new DataReader(_clientSocket.InputStream);
                // Set inputstream options so that we don't have to know the data size
                reader.InputStreamOptions = Partial;
                await reader.LoadAsync(reader.UnconsumedBufferLength);

            }
            catch (Exception exception)
            {
                // If this is an unknown status, 
                // it means that the error is fatal and retry will likely fail.
                if (SocketError.GetStatus(exception.HResult) == SocketErrorStatus.Unknown)
                {
                    throw;
                }

                StatusText.Text = "Receive failed with error: " + exception.Message;
                // Could retry, but for this simple example
                // just close the socket.

                closing = true;
                _clientSocket.Dispose();
                _clientSocket = null;
                connected = false;

            }    
        }




        public InputStreamOptions Partial { get; set; }
    }
}
