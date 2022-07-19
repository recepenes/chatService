using server.Services;

namespace server
{
    class Program
    {
        public static IServerService _serverService { get; set; }
        static void Main(string[] args)
        {
            var server = new ServerService(100);
            _serverService = server;

            Console.Title = "The Server";

            Console.WriteLine("Activating server...");
            _serverService.StartServer();
            Console.WriteLine("Server is active.");

            // Server should wait.
            Console.WriteLine("Press any key to close server.");
            Console.ReadLine();
        }
    }
}
