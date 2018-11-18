using System;
using UnityEngine;

namespace SocialGame.Internal.Network
{
    [Serializable]
    internal sealed class GeneralSettings
    {
        [SerializeField] private Environment _environment = Environment.Development;
        
        [SerializeField] private bool _debugMode = false;

        public bool DebugMode => _debugMode;

        public Environment Environment => _environment;
    }
}
