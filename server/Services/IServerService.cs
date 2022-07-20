using System.Net.Sockets;

namespace server.Services
{
    public interface IServerService
    {

        int Port { get; }

        bool StartServer();
    }
}