using Microsoft.AspNetCore.SignalR;
using UnityBallsCore;
using UnityBallsCore.Command;
using UnityBallsServer.Model;

namespace UnityBallsServer
{
    internal class MainHub : Hub<IClient>, IServer
    {
        private readonly ILogger<MainHub> _logger;
        private readonly IConfiguration _configuration;
        private readonly IGame _game;

        public MainHub(
            ILogger<MainHub> logger,
            IConfiguration configuration,
            IGame game)
        {
            _logger = logger;
            _configuration = configuration;
            _game = game;
        }

        #region IServer

        public async void MoveUser(MoveUserCommand command)
        {
            string connectionId = Context.ConnectionId;

            var networkDelay = _configuration.GetValue<int?>("NetworkDelay");
            if (networkDelay.HasValue)
            {
                await Task.Delay(networkDelay.Value);
            }

            _game.EnqueueCommand(new Command(command, connectionId));
        }

        #endregion

        #region override

        public override Task OnConnectedAsync()
        {
            _logger.LogInformation("New client is connected: {Id}", Context.ConnectionId);

            _game.EnqueueCommand(
                new Command(new AddUserCommand(), Context.ConnectionId));

            return Task.CompletedTask;
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogInformation("Client is disconnected: {Id}", Context.ConnectionId);
            if (exception != null)
            {
                _logger.LogError("Disconnected with exception: {Exception}", exception.Message);
            }

            _game.EnqueueCommand(
                new Command(new RemoveUserCommand(), Context.ConnectionId));

            return Task.CompletedTask;
        }

        #endregion
    }
}
