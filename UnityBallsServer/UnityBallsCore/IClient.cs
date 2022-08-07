using System.Threading.Tasks;
using UnityBallsCore.Message;

namespace UnityBallsCore
{
    public interface IClient
    {
        Task Connect(ConnectMessage message);

        Task UpdateScene(UpdateSceneMessage message);
    }
}
