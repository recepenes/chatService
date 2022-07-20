using server.Services;


namespace chatServiceTest
{
    public class ServerTest
    {
        IServerService _server;
        IServerService _server2;
        [SetUp]
        public void Setup()
        {
            //Arrange
            _server = new ServerService(101);
            _server2 = new ServerService(1022);
        }
        [Test]
        public void Start_Server()
        {
            //Act
            var result = _server.StartServer();

            //Assert
            Assert.IsTrue(result);
        }
    
    }
}
