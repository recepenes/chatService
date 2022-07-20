using client.Services;
using server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            _server.Exit();
        }
        [Test]
        public void ShutDown_Server()
        {
            //Arrange
           _= _server2.StartServer();

            //Act
            _server2.Exit();
            var result = _server2.StartServer();

            //Assert
            Assert.IsTrue(result);

        }
    }
}
