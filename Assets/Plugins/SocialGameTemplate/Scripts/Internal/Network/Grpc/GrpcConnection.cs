#if SGT_GRPC
using System;
using Grpc.Core;
using SocialGame.Network;
using Zenject;
using Debug = UnityEngine.Debug;

namespace SocialGame.Internal.Network.Grpc
{
    internal sealed class GrpcConnection : IInitializable, IGrpcConnection
    {
        [Inject] private GeneralSettings _generalSettings = null;
        
        [Inject] private GrpcSettings _settings = null;

        private GrpcSettings.Channel _envChannel;
        
        void IInitializable.Initialize()
        {
            switch (_generalSettings.Environment)
            {
                case Environment.Production:
                    _envChannel = _settings.ProductionChannel;
                    break;
                case Environment.Staging:
                    _envChannel = _settings.StagingChannel;
                    break;
                case Environment.Development:
                    _envChannel = _settings.DevelopChannel;
                    break;
                default:
                    Debug.unityLogger.LogError(GetType().Name, $"Not implementation {_generalSettings.Environment}");
                    _envChannel = _settings.DevelopChannel;
                    break;
            }
        }
        
        Channel IGrpcConnection.Channel => new Channel(_envChannel.Host, _envChannel.Port, ChannelCredentials.Insecure);

        CallOptions IGrpcConnection.CallOptions => new CallOptions()
            .WithDeadline(DateTime.UtcNow.AddSeconds(_generalSettings.TimeOutSeconds));
    }
}
#endif