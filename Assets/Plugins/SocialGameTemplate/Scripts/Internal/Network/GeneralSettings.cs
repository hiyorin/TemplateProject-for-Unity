using System;
using UnityEngine;

namespace SocialGame.Internal.Network
{
    [Serializable]
    internal sealed class GeneralSettings
    {
        [SerializeField] private Environment _environment = Environment.Development;
        
        [SerializeField] private bool _debugMode = false;

        [SerializeField] private float _timeOutSeconds = 10.0f;

        [SerializeField] private int _retryCount = 0;
        
        public bool DebugMode => _debugMode;

        public Environment Environment => _environment;
        
        public float TimeOutSeconds => _timeOutSeconds;

        public int RetryCount => _retryCount;
    }
}
