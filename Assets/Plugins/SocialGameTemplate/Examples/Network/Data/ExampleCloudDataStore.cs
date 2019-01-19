using Zenject;
using UniRx.Async;
using SocialGame.Network;

namespace Sandbox.Network.Data
{
    internal class ExampleParam
    {
        public string Id;
    }
    
    internal sealed class ExampleCloudDataStore : IExampleDataStore
    {
        [Inject] private IHttpConnection _connection = null;

        public async UniTask<ExampleEntity> Example(string id)
        {
            return await _connection.Post<ExampleParam, ExampleEntity>("example", new ExampleParam() { Id = id });
        }
    }
}
