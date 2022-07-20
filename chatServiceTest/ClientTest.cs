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
        public void Connect_Server_With_One_Client()
        {
            //Arrange
            _ = _server.StartServer();

            //Act
            var result = _client.Connect();

            //Assert
            Assert.IsTrue(result);
        }
        [Test]
        public void Connect_Server_With_Two_Client()
        {
            //Arrange
            var server = _server.StartServer();

            //Act
            var result = _client.Connect();
            var result2 = _client2.Connect();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result);
                Assert.IsTrue(result2);
            });
        }
        [Test]
        public void Send_Message_From_One_Client()
        {
            //Arrange
            _ = _server.StartServer();

            //Act
            var result = _client.Connect();
            _client.SendMessage("Test_Client_1");
            var message = _client.GetMessage();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result);
                Assert.That(message, Does.Contain("Message successfuly delivered."));
            });
        }
        [Test]
        public void Send_Message_From_Two_Client()
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
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result);
                Assert.That(message, Does.Contain("Message successfuly delivered."));

                Assert.IsTrue(result2);
                Assert.That(message2, Does.Contain("Message successfuly delivered."));
            });
        }
        [Test]
        public void Send_Two_Message_In_One_Second_With_One_Client()
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
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result);
                Assert.That(message2, Does.Contain("You can only send 1 message per second," +
                        " next time you will disconeected from the server."));
            });
        }
        [Test]
        public void Second_Failed_Attempt()
        {
            //Arrange
            _ = _server.StartServer();

            //Act
            var result = _client.Connect();
            _client.SendMessage("Test_Client_1");
            _client.SendMessage("Test_Client_2");
            var message1 = _client.GetMessage();
            _client.SendMessage("Test_Client_3");
            var message2 = _client.GetMessage();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.IsTrue(result);
                Assert.That(message1, Does.Contain("You can only send 1 message per second," +
                                " next time you will disconeected from the server."));
                Assert.That(message2, Does.Contain("You are disconnected from server."));
            });
        }
    }
}
