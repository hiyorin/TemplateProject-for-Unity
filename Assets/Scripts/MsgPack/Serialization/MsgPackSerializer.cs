// this file was auto-generated.
using UnityEngine;
namespace MsgPack.Serialization
{
    sealed class MsgPackSerializerRegister
    {
        [RuntimeInitializeOnLoadMethod]
        private static void RuntimeInitializeOnLoadMethod()
        {
            MsgPackSerializer.Register(new MsgPack.Serialization.GeneratedSerializers.DataSerializer(MsgPackSerializer.Context));
        }
    }
}
