using System;
using Zenject;
using UniRx;
using SocialGame.Network;

namespace Sandbox.Network
{
    public class ExampleParam
    {
        public string Id;
    }
    
    public sealed class ExampleCloudDataStore : IExampleDataStore
    {
        [Inject] private IHttpConnection _connection;

        public IObservable<ExampleEntity> Example(string id)
        {
            return _connection
                .Post<ExampleParam, ExampleEntity>("example", new ExampleParam() { Id = id });
        }
    }
}
