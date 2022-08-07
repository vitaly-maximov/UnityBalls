# UnityBalls
A simple multiplayer app with next features:
* An authoritative server on ASP.NET Core SignalR
* Unity3d clients with 2d top view
* Moving of a ball using arrow keys or "WASD"
* Rectangular obstacles
* A kind of client-side prediction

![image](https://user-images.githubusercontent.com/2083367/183298494-bed03c7f-5595-47dd-98f2-1ad321febe1f.png)

## Getting started

* Clone this repo:<br>
<code>git clone https://github.com/vitaly-maximov/UnityBalls.git</code>
* Build and run a server (VS 2022, .Net 6):<br>
![image](https://user-images.githubusercontent.com/2083367/183298963-fcf429f8-bb9f-434e-b2ef-c18269b15a38.png)
* Add <b>UnityBallsHost</b> system variable with server address if it's not <b>https://localhost:5001</b>
* Build and run multiple clients (Unity 2021)
* <b>NetworkDelay</b> can be added to <b>appsettings.json</b> to make server process commands with a delay

App can be downloaded <a href='https://drive.google.com/file/d/1rgXNQg8iqbqcoWTWHMjm5QXiRLnWu3iy/view?usp=sharing'>here</a>
