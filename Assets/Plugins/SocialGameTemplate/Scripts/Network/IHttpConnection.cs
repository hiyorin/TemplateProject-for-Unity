using System;
using UniRx;

namespace SocialGame.Network
{
    public interface IHttpConnection
    {
        IObservable<TResponse> Get<TRequest, TResponse>(string path, TRequest data);
        IObservable<TResponse> Post<TRequest, TResponse>(string path, TRequest data);
    }
}
