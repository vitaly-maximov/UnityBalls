using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System.IO;
using System.Linq;
using UnityBallsCore.Command;
using UnityBallsCore.Message;
using UnityBallsCore.Model;
using UnityBallsServer.Model;

namespace UnityBallsTests
{
    public class GameTests
    {
        private IGame _game = null!;

        [SetUp]
        public void Setup()
        {
            ILogger<Game> logger = new LoggerFactory().CreateLogger<Game>();

            IConfiguration configuration;
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            {
                writer.Write("{\"Seed\": \"123\"}");
                writer.Flush();

                stream.Seek(0, SeekOrigin.Begin);

                configuration = new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .Build();
            }

            _game = new Game(logger, configuration);
        }

        [Test]
        public void TestAddUser()
        {
            _game.EnqueueCommand(new Command(new AddUserCommand(), "abc"));
            _game.EnqueueCommand(new Command(new AddUserCommand(), "cba"));

            var messages = _game.Process().ToArray();

            Assert.That(messages[0].ConnectionId, Is.EqualTo("abc"));
            Assert.That(((ConnectMessage)messages[0].ActualMessage).UserId, Is.EqualTo(1));
            Assert.That(((ConnectMessage)messages[0].ActualMessage).World.Obstacles.Count, Is.GreaterThanOrEqualTo(4));

            Assert.That(messages[1].ConnectionId, Is.EqualTo("cba"));
            Assert.That(((ConnectMessage)messages[1].ActualMessage).UserId, Is.EqualTo(2));

            Assert.That(messages[2].ConnectionId, Is.EqualTo(null));
            Assert.That(((UpdateSceneMessage)messages[2].ActualMessage).State.Users.Count, Is.EqualTo(2));
            Assert.That(((UpdateSceneMessage)messages[2].ActualMessage).State.Users[0].Position, Is.EqualTo(Vector2f.Empty));
        }

        [Test]
        public void TestRemoveUser()
        {
            _game.EnqueueCommand(new Command(new AddUserCommand(), "abc"));
            _game.EnqueueCommand(new Command(new RemoveUserCommand(), "abc"));

            var messages = _game.Process().ToArray();

            Assert.That(((UpdateSceneMessage)messages[1].ActualMessage).State.Users.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestMoveUser()
        {
            _game.EnqueueCommand(new Command(new AddUserCommand(), "abc"));
            _game.EnqueueCommand(new Command(new MoveUserCommand(Vector2f.Up), "abc"));

            var messages = _game.Process().ToArray();

            var user = ((UpdateSceneMessage)messages[1].ActualMessage).State.Users[0];
            Assert.That(user.Position, Is.EqualTo(new Vector2f(0, user.Speed)));
        }
    }
}
