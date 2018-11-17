using System;
using UnityEngine;

namespace SocialGame.Network
{
    [Serializable]
    public sealed class GeneralSettings
    {        
        [SerializeField] private bool _debugMode = false;

        public bool DebugMode
        {
            get { return _debugMode; }
        }
    }
}
