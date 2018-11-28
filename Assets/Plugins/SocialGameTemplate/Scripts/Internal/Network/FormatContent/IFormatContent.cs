using System;
using UnityEngine.Networking;
using UniRx;

namespace SocialGame.Internal.Network.FormatContent
{
    internal interface IFormatContent
    {
        void SetRequestHeader(UnityWebRequest request);
        byte[] Serialize<T>(T data);
        IObservable<T> Deserialize<T>(byte[] data);
    }
}
