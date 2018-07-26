using System;
using UniRx;

namespace SocialGame.Network
{
    public interface IApiConnection
    {
        IObservable<object> OnUpdateMasterAsObservable();
    }
    /*
    public sealed class ApiConnection : IApiConnection
    {

    }
    */
}
