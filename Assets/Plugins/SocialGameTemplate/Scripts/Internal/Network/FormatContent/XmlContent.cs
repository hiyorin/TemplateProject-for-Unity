using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine.Networking;
using UniRx;

namespace SocialGame.Internal.Network.FormatContent
{
    public sealed class XmlContent : IFormatContent
    {
        #region IFormatContent implementation
        void IFormatContent.SetRequestHeader(UnityWebRequest request)
        {
            request.SetRequestHeader("Content-Type", "application/xml; charset=UTF-8");		
        }

        byte[] IFormatContent.Serialize<T>(T data)
        {
            using (var stream = new MemoryStream())
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stream, data);
                return stream.ToArray();
            }
        }

        IObservable<T> IFormatContent.Deserialize<T>(byte[] data)
        {
            return Observable.Start(() =>
            {
                using (var stream = new MemoryStream(data))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(stream);
                }
            });
        }
        #endregion
    }
}
