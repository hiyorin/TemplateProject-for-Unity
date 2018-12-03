using System;
using UnityEngine.Networking;
using MsgPack.Serialization;
using UniRx;

namespace SocialGame.Internal.Network.FormatContent
{
	public sealed class MsgPackContent : IFormatContent
	{
		#region IFormatContent implementation
		void IFormatContent.SetRequestHeader(UnityWebRequest request)
		{
			request.SetRequestHeader("Content-Type", "application/x-msgpack; charset=UTF-8");
		}

		byte[] IFormatContent.Serialize<T>(T data)
		{
			var serializer = MsgPackSerializer.Get<T>();
			return serializer.PackSingleObject(data);
		}

		IObservable<T> IFormatContent.Deserialize<T>(byte[] data)
		{
			var serializer = MsgPackSerializer.Get<T>();
			return Observable.Start(() => serializer.UnpackSingleObject(data));
		}
		#endregion
	}
}
