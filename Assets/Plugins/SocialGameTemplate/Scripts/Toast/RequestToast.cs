
namespace SocialGame.Toast
{
    public sealed class RequestToast
    {
        public readonly string Name;

        public readonly object Param;

        public RequestToast(ToastType type, object param) : this(type.ToString(), param)
        {
            
        }

        public RequestToast(string name, object param)
        {
            Name = name;
            Param = param;
        }
    }
}
