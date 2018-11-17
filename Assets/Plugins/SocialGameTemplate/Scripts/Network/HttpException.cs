using System;
using System.Net;

namespace SocialGame.Network
{
    public sealed class HttpException : Exception
    {
        public readonly HttpStatusCode Status;

        public readonly int ResponseCode;
        
        public HttpException(int responseCode, string message) : base(message)
        {
            Status = (HttpStatusCode)responseCode;
            ResponseCode = responseCode;
        }

        public HttpException(HttpStatusCode statusCode, string message) : base(message)
        {
            Status = statusCode;
            ResponseCode = (int)statusCode;
        }
    }
}
