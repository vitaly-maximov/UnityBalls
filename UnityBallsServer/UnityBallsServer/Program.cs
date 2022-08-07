using TestSignalR;
using UnityBallsServer;
using UnityBallsServer.Model;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services
    .AddSingleton<IGame, Game>()
    .AddHostedService<GameService>()
    .AddCors()
    .AddSignalR();

var app = builder.Build();

app.UseCors(b => b
    .SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseHttpsRedirection();

app.MapHub<MainHub>("/game");
app.Run();