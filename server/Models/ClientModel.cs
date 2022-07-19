using System.Net.Sockets;

namespace server.Models
{
    public class ClientModel
    {
        public Socket Socket { get; set; }
        public bool isSended { get; set; }
    }
}
