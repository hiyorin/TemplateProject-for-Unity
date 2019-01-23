#if SGT_GRPC
using Helloworld;
using SocialGame.Network;
using UniRx.Async;
using Zenject;

namespace Sandbox.Network.Data
{
    internal interface IHelloworldDataStore
    {
        UniTask<string> SayHello(string name);
    }
    
    internal sealed class HelloworldDataStore : IHelloworldDataStore
    {
        [Inject] private IGrpcConnection _connection = null;

        async UniTask<string> IHelloworldDataStore.SayHello(string name)
        {
            var channel = _connection.Channel;
            var client = new Greeter.GreeterClient(channel);
            var reply = await client.SayHelloAsync(new HelloRequest() {Name = name}, _connection.CallOptions);
            await channel.ShutdownAsync();
            return reply.Message;
        }
    }
}
#endif
