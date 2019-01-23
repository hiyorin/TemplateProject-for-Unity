using UniRx.Async;

namespace SocialGame.Network
{
    public interface IHttpConnection
    {
        UniTask<TResponse> Get<TRequest, TResponse>(string path, TRequest data);
        UniTask<TResponse> Post<TRequest, TResponse>(string path, TRequest data);
    }
}
