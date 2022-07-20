using System.Net.Sockets;

namespace server.Services
{
    public interface IServerService
    {
        byte[] Buffer { get; set; }
        int BufferSize { get; }
        int PORT { get; }
        Socket Socket { get; }

        void Exit();
        bool StartServer();
    }
}