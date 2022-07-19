using client.Services;

namespace client
{
    class Program
    {
        public static IClientService _clientService { get; set; }
        static void Main(string[] args)
        {
            var client = new ClientService(100);

            _clientService = client;

            Console.Title = $"The Client #{_clientService.ID}";

            Console.WriteLine($"{_clientService.ID} trying to connect...");
            _clientService.Connect();
            Console.WriteLine($"{_clientService.ID} CONNECTED.");

            Console.WriteLine($"{_clientService.ID} started to listening...");
            _clientService.Listen();

        }
    }
}