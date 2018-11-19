using System;
using UnityEngine;

namespace SocialGame.Internal.Network
{
    [Serializable]
    public sealed class NetworkSettings : ScriptableObject
    {
        public const string FileName = "NetworkSettings";
        
        [SerializeField] private GeneralSettings _general;

        [SerializeField] private HttpSettings _http;

        internal GeneralSettings General => _general;

        internal HttpSettings Http => _http;
    }
}
