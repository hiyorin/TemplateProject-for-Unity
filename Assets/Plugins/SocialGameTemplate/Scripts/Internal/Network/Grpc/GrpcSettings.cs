#if SGT_GRPC
using System;
using UnityEngine;

namespace SocialGame.Internal.Network.Grpc
{
    [Serializable]
    internal sealed class GrpcSettings
    {
        [Serializable]
        internal sealed class Channel
        {
            [SerializeField] private string _host = "localhost";
    
            [SerializeField] private int _port = 8080;
    
            public string Host => _host;
    
            public int Port => _port;
        }

        [SerializeField] private Channel _productionChannel = null;

        [SerializeField] private Channel _stagingChannel = null;

        [SerializeField] private Channel _developChannel = null;
        
        public Channel ProductionChannel => _productionChannel;

        public Channel StagingChannel => _stagingChannel;

        public Channel DevelopChannel => _developChannel;
    }
}
#endif
