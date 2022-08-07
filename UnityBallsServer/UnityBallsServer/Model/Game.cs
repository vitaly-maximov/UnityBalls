using System.Collections.Concurrent;
using UnityBallsCore.Command;
using UnityBallsCore.Common;
using UnityBallsCore.Message;
using UnityBallsCore.Model;
using UnityBallsServer.Model;

namespace TestSignalR
{
    internal partial class Game : IGame
    {
        private record Obstacle(Rect2f Bounds, uint ColorArgb);        

        private static readonly Rect2f World = new(-50, -50, 100, 100);

        private static readonly uint[] s_colors =
        {
            0xff880000,
            0xff008800,
            0xff000088,
            0xff888800,
            0xff008888,
            0xff880088
        };

        private readonly ILogger<Game> _logger;
        private readonly Random _random;

        private readonly ConcurrentQueue<Command> _commands = new();
        private readonly Dictionary<string, User> _users = new();
        private readonly List<Obstacle> _obstacles;

        private int _serialUserId = 0;

        public Game(
            ILogger<Game> logger,
            IConfiguration configuration)
        {
            _logger = logger;

            int? seed = configuration.GetValue<int?>("Seed");
            _random = new Random(seed ?? (int)DateTime.Now.Ticks);

            _obstacles = CreateObstacles();
        }

        #region IGame

        public void EnqueueCommand(Command command) => 
            _commands.Enqueue(command);

        public IReadOnlyList<Message> Process()
        {
            var messages = new List<Message>();

            int length = _commands.Count;
            for (int i = 0; i < length;)
            {
                if (!_commands.TryDequeue(out Command? command))
                {
                    continue;
                }
                
                Func<Command, IEnumerable<Message>> action = command.ActualCommand switch
                {
                    AddUserCommand => AddUser,
                    RemoveUserCommand => RemoveUser,
                    MoveUserCommand => MoveUser,
                    _ => throw new InvalidOperationException(
                        $"Unknown command: {command.ActualCommand.GetType()}")
                };

                messages.AddRange(action(command));
                ++i;
            }
            
            if (length > 0)
            {
                messages.AddRange(UpdateScene());
            }
            
            return messages;
        }

        #endregion

        #region private methods

        private IEnumerable<Message> AddUser(Command command)
        {
            if (_users.ContainsKey(command.ConnectionId))
            {
                _logger.LogError(
                    "AddUser() failed. There is a user with connection id: {Id}", 
                    command.ConnectionId);

                yield break;
            }

            var user = new User(++_serialUserId, command.ConnectionId)
            {
                Speed = 0.15f + 0.1f * _random.NextSingle(),
                ColorArgb = s_colors[_random.Next(s_colors.Length)]
            };

            _users[command.ConnectionId] = user;

            _logger.LogInformation(
                "Add a user {UserId} with connection id: {ConnectionId}", 
                user.Id, 
                command.ConnectionId);

            var world = new WorldDto
            {             
                Width = World.Width,
                Height = World.Height
            };

            foreach (var obstacle in _obstacles)
            {
                world.Obstacles.Add(
                    new ObstacleDto
                    {
                        Bounds = obstacle.Bounds,
                        ColorArgb = obstacle.ColorArgb
                    });
            }

            yield return new Message(
                new ConnectMessage(user.Id, world), 
                command.ConnectionId);
        }

        private IEnumerable<Message> RemoveUser(Command command)
        {
            if (!_users.ContainsKey(command.ConnectionId))
            {
                _logger.LogError(
                    "RemoveUser() failed. There is no user with connection id: {Id}", 
                    command.ConnectionId);

                yield break;
            }

            var user = _users[command.ConnectionId];
            _users.Remove(command.ConnectionId);

            _logger.LogInformation(
                "Remove a user {UserId} with connection id: {ConnectionId}", 
                user.Id, 
                command.ConnectionId);
        }

        private IEnumerable<Message> MoveUser(Command command)
        {
            if (!_users.ContainsKey(command.ConnectionId))
            {
                _logger.LogError(
                    "MoveUser() failed. There is no user with connection id: {Id}", 
                    command.ConnectionId);

                yield break;
            }

            var user = _users[command.ConnectionId];

            var moveUserCommand = (MoveUserCommand)command.ActualCommand;
            if (moveUserCommand.Direction == Vector2f.Empty)
            {
                _logger.LogWarning("MoveUser() failed. Direction is unknown.");

                yield break;
            }

            user.Direction = moveUserCommand.Direction;

            user.Position = Physics.Move(
                user.Position,
                user.Direction,
                user.Speed,
                _obstacles.Select(obstacle => obstacle.Bounds));
        }

        private IEnumerable<Message> UpdateScene()
        {
            StateDto state = new();
            foreach (var user in _users.Values)
            {
                state.Users.Add(new UserDto
                {
                    Id = user.Id,
                    Position = user.Position,
                    Direction = user.Direction,
                    Speed = user.Speed,
                    ColorArgb = user.ColorArgb
                });
            }

            yield return new Message(new UpdateSceneMessage(state));
        }

        private List<Obstacle> CreateObstacles(int count = 15)
        {
            List<Obstacle> obstacles = new();

            // preserve some space for start position
            obstacles.Add(new Obstacle(new Rect2f(-1, -1, 2, 2), ColorArgb: 0));

            // add walls
            uint wallColor = s_colors[_random.Next(s_colors.Length)];
            obstacles.AddRange(new[] {
                new Obstacle(new Rect2f(World.Left - 1, World.Bottom - 1, 1, World.Height + 2), wallColor),
                new Obstacle(new Rect2f(World.Right, World.Bottom - 1, 1, World.Height + 2), wallColor),
                new Obstacle(new Rect2f(World.Left, World.Bottom - 1, World.Width, 1), wallColor),
                new Obstacle(new Rect2f(World.Left, World.Top, World.Width, 1), wallColor)
            });

            for (int i = 0; i < count;)
            {
                int width = _random.Next(2, (int)World.Width / 10);
                int height = _random.Next(2, (int)World.Height / 10);

                float x = World.Left + (World.Width - width) * _random.NextSingle();
                float y = World.Bottom + (World.Height - height) * _random.NextSingle();

                Rect2f bounds = new(x, y, width, height);
                if (obstacles.All(obstacle => !obstacle.Bounds.IntersectsWith(bounds)))
                {
                    uint color = s_colors[_random.Next(s_colors.Length)];
                    obstacles.Add(new Obstacle(bounds, color));

                    ++i;
                }
            }

            obstacles.RemoveAt(0);
            return obstacles;
        }

        #endregion
    }
}
