
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
        public DateTime LastMessageTime { get; set; }
        public int TryCount { get; set; }

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
                GetMessage();
            }
        }

        public string GetMessage()
        {
            byte[] recBuf = new byte[254];
            Socket.Receive(recBuf);
            string message = Encoding.ASCII.GetString(recBuf);
            Console.WriteLine("Response: " + message);
            return message;
        }

        public void SendMessage(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            Socket.Send(buffer, 0, buffer.Length, SocketFlags.None);
            LastMessageTime = DateTime.Now;
        }

        private string SetMessage()
        {
            Console.WriteLine("Type your message: ....");
            return Console.ReadLine();
        }

        public void Exit()
        {
            Socket.Shutdown(SocketShutdown.Both);
            Socket.Close();
            Environment.Exit(0);
        }
    }
}
