using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using UnityBallsCore;
using UnityBallsCore.Message;

namespace UnityBallsServer.Model
{
    internal class GameService : BackgroundService
    {
        private readonly ILogger<GameService> _logger;
        private readonly IGame _game;
        private readonly IHubContext<MainHub, IClient> _hub;

        public GameService(
            ILogger<GameService> logger,
            IGame game,
            IHubContext<MainHub, IClient> hub)
        {
            _logger = logger;
            _game = game;
            _hub = hub;
        }

        #region override

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            const int delay = 15;

            var stopwatch = new Stopwatch();
            while (!stoppingToken.IsCancellationRequested)
            {
                stopwatch.Restart();

                foreach (Message message in _game.Process())
                {
                    IClient client = message.ConnectionId == null
                        ? _hub.Clients.All
                        : _hub.Clients.Client(message.ConnectionId);

                    Task _ = message.ActualMessage switch
                    {
                        ConnectMessage m => client.Connect(m),
                        UpdateSceneMessage m => client.UpdateScene(m),
                        _ => throw new InvalidOperationException($"Unknown message: {message.GetType()}")
                    };
                }

                stopwatch.Stop();

                if (stopwatch.ElapsedMilliseconds > delay)
                {
                    _logger.LogWarning(
                        "Game iteration is too long: {Time}",
                        stopwatch.ElapsedMilliseconds);
                }
                else
                {
                    await Task.Delay(
                        delay - (int)stopwatch.ElapsedMilliseconds,
                        stoppingToken);
                }
            }
        }

        #endregion
    }
}
