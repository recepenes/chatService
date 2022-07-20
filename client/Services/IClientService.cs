using System.Net.Sockets;

namespace client.Services
{
    public interface IClientService
    {
        int ID { get; }
        int Port { get; }

        bool Connect();
        void Exit();
        string GetMessage();
        void Listen();
        void SendMessage(string data);
    }
}