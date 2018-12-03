// this file was auto-generated.
using MsgPack.Serialization.GeneratedSerializers;
namespace MsgPack.Serialization
{
    public static class MsgPackSerializer
    {
        static readonly SerializationContext Context = new SerializationContext
        {
            EnumSerializationMethod = EnumSerializationMethod.ByUnderlyingValue,
            SerializationMethod = SerializationMethod.Array,
        };
        static MsgPackSerializer()
        {
            Context.Serializers.Register(new DataSerializer(Context));
        }
        public static MessagePackSerializer<T> Get<T>()
        {
            return Context.GetSerializer<T>();
        }
    }
}
