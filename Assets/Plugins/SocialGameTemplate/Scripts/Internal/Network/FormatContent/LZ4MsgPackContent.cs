using System;
using UnityEngine.Networking;
using MessagePack;
using UniRx;

namespace SocialGame.Internal.Network.FormatContent
{
	internal sealed class LZ4MsgPackContent : IFormatContent
	{
		#region IFormatContent implementation
		void IFormatContent.SetRequestHeader(UnityWebRequest request)
		{
			request.SetRequestHeader("Content-Type", "application/x-lz4msgpack; charset=UTF-8");
		}

		byte[] IFormatContent.Serialize<T>(T data)
		{
			return MessagePackSerializer.Serialize(data);
		}

		IObservable<T> IFormatContent.Deserialize<T>(byte[] data)
		{
			return Observable.Start(() => LZ4MessagePackSerializer.Deserialize<T>(data));
		}
		#endregion
	}
}