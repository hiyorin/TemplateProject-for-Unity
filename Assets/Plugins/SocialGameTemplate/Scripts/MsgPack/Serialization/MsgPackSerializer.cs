
namespace MsgPack.Serialization
{
    public static class MsgPackSerializer
    {
        public static readonly SerializationContext Context = new SerializationContext
        {
            EnumSerializationMethod = EnumSerializationMethod.ByUnderlyingValue,
            SerializationMethod = SerializationMethod.Array,
        };
        
        public static MessagePackSerializer<T> Get<T>()
        {
            return Context.GetSerializer<T>();
        }

        public static bool Register<T>(MessagePackSerializer<T> serializer)
        {
            return Context.Serializers.Register(serializer);
        }
    }
}
