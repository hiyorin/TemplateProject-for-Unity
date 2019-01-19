using UniRx.Async;

namespace Sandbox.Network.Data

{
    internal interface IExampleDataStore
    {
        UniTask<ExampleEntity> Example(string id);
    }
}
