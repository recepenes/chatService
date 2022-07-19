using client.Services;
using server.Services;

namespace ChatConsoleTest
{
    public class ClientTest
    {
        IClientService _client, _client2;
        IServerService _server;
        [SetUp]
        public void Setup()
        {
            //Arrange
            _client = new ClientService(100);
            _client2 = new ClientService(100);
            _server = new ServerService(100);
        }

        [Test]
        public void Connect_Server_One_Client()
        {
            //Arrange
            _ = _server.StartServer();
            //Act
            var result = _client.Connect();

            //Assert
            Assert.IsTrue(result);
        }
        [Test]
        public void Connect_Server_Two_Client()
        {
            //Arrange
            var client2 = new ClientService(100);
            var server = _server.StartServer();
            //Act
            var result = _client.Connect();
            var result2 = client2.Connect();

            //Assert
            Assert.IsTrue(result);
            Assert.IsTrue(result2);
        }
        [Test]
        public void Send_Message_One_Client()
        {
            //Arrange
            _ = _server.StartServer();
            //Act
            var result = _client.Connect();
            _client.SendMessage("Test_Client_1");
            var message = _client.GetMessage();
            //Assert
            Assert.IsTrue(result);
            Assert.IsTrue(message.Contains("Message successfuly delivered."));
        }
        [Test]
        public void Send_Message_Two_Client()
        {
            //Arrange
            _ = _server.StartServer();
            //Act
            var result = _client.Connect();
            _client.SendMessage("Test_Client_1");
            var message = _client.GetMessage();

            var result2 = _client2.Connect();
            _client2.SendMessage("Test_Client_2");
            var message2 = _client2.GetMessage();
            //Assert
            Assert.IsTrue(result);
            Assert.IsTrue(message.Contains("Message successfuly delivered."));

            Assert.IsTrue(result2);
            Assert.IsTrue(message2.Contains("Message successfuly delivered."));
        }
        [Test]
        public void Send_Two_Message_One_Second()
        {
            //Arrange
            _ = _server.StartServer();
            //Act
            var result = _client.Connect();
            _client.SendMessage("Test_Client_1");
            var message = _client.GetMessage();
            _client.SendMessage("Test_Client_2");
            var message2 = _client.GetMessage();
            //Assert
            Assert.IsTrue(result);
            Assert.IsTrue(message2.Contains("You can only send 1 message per second," +
                    " next time you will disconeected from the server."));
        }
    }
}
