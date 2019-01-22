#if SGT_GRPC
using System;
using Grpc.Core;
using SocialGame.Network;
using Zenject;

namespace SocialGame.Internal.Network.Grpc
{
    internal sealed class GrpcConnection : IInitializable, IGrpcConnection
    {
        [Inject] private GeneralSettings _generalSettings = null;
        
        [Inject] private GrpcSettings _settings = null;

        private GrpcSettings.Channel _envChannel;
        
        void IInitializable.Initialize()
        {
            _envChannel = _settings.ProductionChannel;
        }
        
        Channel IGrpcConnection.Channel => new Channel(_envChannel.Host, _envChannel.Port, ChannelCredentials.Insecure);

        CallOptions IGrpcConnection.CallOptions => new CallOptions()
            .WithDeadline(DateTime.UtcNow.AddSeconds(_generalSettings.TimeOutSeconds));
    }
}
#endif