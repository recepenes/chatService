using System.Net.Sockets;

namespace client.Services
{
    public interface IClientService
    {
        int ID { get; }
        int PORT { get; }
        Socket Socket { get; }
        int TryCount { get; set; }

        bool Connect();
        void Exit();
        string GetMessage();
        void Listen();
        void SendMessage(string data);
    }
}