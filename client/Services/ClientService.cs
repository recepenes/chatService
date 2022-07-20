
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace client.Services
{
    public class ClientService : IClientService
    {
        public int PORT { get; private set; }
        public Socket Socket { get; private set; }

        public int ID { get; private set; }
        public int TryCount { get; set; }
        private static ManualResetEvent sendDone =
      new ManualResetEvent(false);
        public ClientService(int port)
        {
            ID = new Random().Next(0, 100);
            PORT = port;
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public bool Connect()
        {
            while (Socket.Connected == false)
            {
                try
                {
                    Socket.Connect(IPAddress.Loopback, PORT);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("exception: {0}", ex.Message);
                    return false;
                }
            }
            return false;
        }

        public void Listen()
        {
            // we should listen our client, so this is an infinite loop.
            while (true)
            {
                string message = SetMessage();
                SendMessage(message);
                Console.WriteLine("Response: " + GetMessage());
            }
        }

        public string GetMessage()
        {
            Thread.Sleep(5);
            try
            {
                byte[] recBuf = new byte[254];
                Socket.Receive(recBuf);
                string message = Encoding.ASCII.GetString(recBuf);
                return message;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }




        private string SetMessage()
        {
            Console.WriteLine("Type your message: ....");
            return Console.ReadLine();

        }

        public void SendMessage(string data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.  
            Socket.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), Socket);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = client.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to server.", bytesSent);

                // Signal that all bytes have been sent.  
                sendDone.Set();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        public void Exit()
        {
            Socket.Shutdown(SocketShutdown.Both);
            Socket.Close();
            Environment.Exit(0);
        }
    }
}
