using System;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UniRx;

namespace SocialGame.Internal.Network.FormatContent
{
    public sealed class JsonContent : IFormatContent
    {
        #region IFormatContent implementation
        void IFormatContent.SetRequestHeader(UnityWebRequest request)
        {
            request.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");		
        }

        byte[] IFormatContent.Serialize<T>(T data)
        {
            return Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
        }

        IObservable<T> IFormatContent.Deserialize<T>(byte[] data)
        {
            return Observable.Start(() => JsonUtility.FromJson<T>(Encoding.UTF8.GetString(data)));
        }
        #endregion
    }
}