using System;
using UnityEngine;

namespace SocialGame.Internal.Network.gRPC
{
    [Serializable]
    internal sealed class gRPCSettings
    {
        [SerializeField] private string _host = "localhost";

        [SerializeField] private int _port = 8080;

        public string Host => _host;

        public int Port => _port;
    }
}