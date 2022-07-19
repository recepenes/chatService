using System.Net.Sockets;

namespace ServerProject
{
    public interface IServerService
    {
        byte[] Buffer { get; set; }
        int BufferSize { get; }
        int PORT { get; }
        Socket Socket { get; }

        bool StartServer();
    }
}