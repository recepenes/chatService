using server.Models;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace server.Services
{
    public class ServerService : IServerService
    {
        public int PORT { get; private set; }
        public Socket Socket { get; private set; }
        public int BufferSize { get; private set; }
        public byte[] Buffer { get; set; }
        private DateTime LastReceivedTime;
        private List<ClientModel> Clients = new();
        public ServerService(int port)
        {
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            PORT = port;
            BufferSize = 2048;
            Buffer = new byte[BufferSize];
        }

        public bool StartServer()
        {
            try
            {
                IPEndPoint endPoint = new(IPAddress.Any, PORT);
                Socket.Bind(endPoint);

                Socket.Listen(0);
                Socket.BeginAccept(Connect, null);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

        }

        private void Connect(IAsyncResult result)
        {
            Socket socket;
            try
            {
                // accept the connection and set to a new socket.
                socket = Socket.EndAccept(result);
                Clients.Add(new ClientModel() { Socket = socket, isSended = false });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                return;
            }

            socket.BeginReceive(Buffer, 0, BufferSize, SocketFlags.None, Listen, socket);
            Socket.BeginAccept(Connect, null);
        }

        private void Listen(IAsyncResult result)
        {
            Socket current = (Socket)result.AsyncState;
            int received;
            try
            {
                // get the text.
                received = current.EndReceive(result);
            }
            catch (Exception ex)
            {
                current.Close();
                Console.WriteLine("Error: {0}", ex.Message);
                return;
            }
            GetMessage(received);

            if (!CheckMessageGap(current)) return;

            SendResponseMessage(current, "Message successfuly delivered.");

            // Calling same method again, recursive for obvious reasons...
            current.BeginReceive(Buffer, 0, BufferSize, SocketFlags.None, Listen, current);
        }

        private void GetMessage(int received)
        {
            byte[] recBuf = new byte[received];
            Array.Copy(Buffer, recBuf, received);
            string message = Encoding.ASCII.GetString(recBuf);
            Console.WriteLine("Message: " + message);
        }
        private bool CheckMessageGap(Socket currentSocket)
        {
            var timeDifferent = (DateTime.Now - LastReceivedTime).TotalSeconds;
            LastReceivedTime = DateTime.Now;
            if (timeDifferent <= 1)
            {
                var clientIndex = Clients.FindIndex(x => x.Socket == currentSocket);
                if (Clients[clientIndex].isSended)
                {
                    SendResponseMessage(currentSocket, "Your are disconnected from server.");
                    Thread.Sleep(5000);
                    currentSocket.Close();
                    return false;
                }
                SendResponseMessage(currentSocket, "You can only send 1 message per second," +
                    " next time you will disconeected from the server.");
                Clients[clientIndex].isSended = true;
            }
            return true;
        }
        private void SendResponseMessage(Socket socket, string message)
        {
            var response = Encoding.ASCII.GetBytes(message);
            socket.Send(response);
        }
    }
}