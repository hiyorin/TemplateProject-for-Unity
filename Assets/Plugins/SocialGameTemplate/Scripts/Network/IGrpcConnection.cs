#if SGT_GRPC
using Grpc.Core;

namespace SocialGame.Network
{
    public interface IGrpcConnection
    {
        Channel Channel { get; }
        
        CallOptions CallOptions { get; }
    }
}
#endif
