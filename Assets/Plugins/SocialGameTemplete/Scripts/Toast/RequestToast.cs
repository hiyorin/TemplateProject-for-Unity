using System;

namespace SocialGame.Toast
{
    public sealed class RequestToast
    {
        public readonly ToastType Type;

        public readonly object Param;

        public RequestToast(ToastType type, object param)
        {
            Type = type;
            Param = param;
        }
    }
}
