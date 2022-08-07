using Microsoft.AspNetCore.SignalR.Client;
using UnityBallsCore;
using UnityBallsCore.Command;
using UnityBallsCore.Common;
using UnityBallsCore.Message;
using UnityBallsCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Physics = UnityBallsCore.Common.Physics;

public class MainBehavior : MonoBehaviour, IClient
{
    private HubConnection _hubConnection;

    private readonly Dictionary<int, User> _users = new();
    private readonly List<Obstacle> _obstacles = new();

    private int _userId;
    private long _lastUpdateTicks = 0;

    [SerializeField]
    private GameObject _userPrefab;

    [SerializeField]
    private GameObject _obstaclePrefab;

    public void Start()
    {
        if ((_userPrefab == null) || (_obstaclePrefab == null))
        {
            Debug.LogError("User or obstacle prefab is absent.");
            return;
        }

        string host = Environment.GetEnvironmentVariable("UnityBallsHost", EnvironmentVariableTarget.Machine);
        if (string.IsNullOrEmpty(host))
        {
            host = "https://localhost:5001";
        }

        _hubConnection = new HubConnectionBuilder()
            .WithUrl($"{host}/game")
            .Build();

        _hubConnection.On<ConnectMessage>(nameof(Connect), Connect);
        _hubConnection.On<UpdateSceneMessage>(nameof(UpdateScene), UpdateScene);

        EstablishConnection();
    }

    public void Update()
    {
        const int delay = 15;

        long ticks = DateTime.Now.Ticks;
        if (new TimeSpan(ticks - _lastUpdateTicks).TotalMilliseconds < delay)
        {
            return;
        }
        _lastUpdateTicks = ticks;

        if (!EstablishConnection())
        {
            return;
        }

        if (!_users.TryGetValue(_userId, out User user))
        {
            return;
        }

        var direction = new Vector2f(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical"));

        if (direction != Vector2f.Empty)
        {
            _hubConnection
                .SendAsync(nameof(IServer.MoveUser), new MoveUserCommand(direction))
                .ContinueWith(HandleTaskException);

            // lag compensation
            var position = new Vector2f(
                user.GameObject.transform.position.x,
                user.GameObject.transform.position.z);

            var next = Physics.Move(
                position,
                direction,
                user.Speed,
                _obstacles.Select(obstacle => obstacle.Bounds));

            user.GameObject.transform.position = new Vector3(next.X, 0, next.Y);
        }
    }

    void LateUpdate()
    {
        if (_users.TryGetValue(_userId, out User user))
        {
            var userPosition = user.GameObject.transform.position;
            var cameraPosition = Camera.main.transform.position;

            var a = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
            var b = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            var c = (b - a) / 2f;

            var factor = 0.5f;
            var offset = Math.Min(factor * c.x, factor * c.z);
            var dx = c.x - offset;
            var dz = c.z - offset;

            Camera.main.transform.position = new Vector3(
                cameraPosition.x.Clamp(userPosition.x - dx, userPosition.x + dx),
                cameraPosition.y,
                cameraPosition.z.Clamp(userPosition.z - dz, userPosition.z + dz));
        }
    }

    public void OnApplicationQuit() => _hubConnection?
        .StopAsync()
        .ContinueWith(HandleTaskException);

    #region IClient

    public Task Connect(ConnectMessage message)
    {
        Debug.Log($"Connected with id={message.UserId}");

        _userId = message.UserId;
        InitialzieWorld(message.World);

        return Task.CompletedTask;
    }

    public Task UpdateScene(UpdateSceneMessage message)
    {        
        foreach (UserDto userDto in message.State.Users)
        {
            if (!_users.TryGetValue(userDto.Id, out User user))
            {
                _users[userDto.Id] = user = User.Create(userDto, _userPrefab);
            }
            else if (userDto.Id != _userId)
            {
                user.Update(userDto);
            }            
        }

        var toRemove = _users
            .Select(user => user.Key)
            .Except(message.State.Users.Select(user => user.Id))
            .ToArray();

        foreach (var userId in toRemove)
        {
            _users[userId].Dispose();
            _users.Remove(userId);
        }

        return Task.CompletedTask;
    }

    #endregion

    #region private methods

    private bool EstablishConnection()
    {
        if (_hubConnection == null)
        {
            return false;
        }

        if (_hubConnection.State == HubConnectionState.Connected)
        {
            return true;
        }

        if (_hubConnection.State == HubConnectionState.Disconnected)
        {
            _hubConnection
                .StartAsync()
                .ContinueWith(HandleTaskException);
        }        

        return false;
    }

    private void HandleTaskException(Task task)
    {
        if (task.IsFaulted)
        {
            Debug.LogException(task.Exception);
        }
    }

    private void InitialzieWorld(WorldDto world)
    {
        var toDestroy = Enumerable.Concat<IDisposable>(_users.Values, _obstacles);
        foreach (var item in toDestroy)
        {
            item.Dispose();
        }

        _users.Clear();
        _obstacles.Clear();

        transform.localScale = 
            new Vector3(world.Width / 10, 1, world.Height / 10);

        var renderer = GetComponent<Renderer>();
        renderer.material.SetTextureScale(
            "_MainTex",
            new Vector2(world.Width / 4, world.Height / 4));

        _obstacles.AddRange(
            world.Obstacles.Select(
                obstacleDto => Obstacle.Create(obstacleDto, _obstaclePrefab)));
    }

    #endregion
}
