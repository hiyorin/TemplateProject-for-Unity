using System;
using UnityEngine;

namespace SocialGame.Network
{
    [Serializable]
    public sealed class HttpSettings
    {
        public enum Format
        {
            JSON,
            MessagePack,
        }
        
        [SerializeField] private string _domain = "https://www.google.co.jp/";

        [SerializeField] private Format _dataFormat = Format.JSON;
        
        [SerializeField] private bool _useChunkedTransfer = true;

        [SerializeField] private float _timeOutSeconds = 10.0f;

        [SerializeField] private int _retryCount = 0;

        public string Domain
        {
            get { return _domain; }
        }

        public Format DataFormat
        {
            get { return _dataFormat; }
        }
        
        public bool UseChunkedTransfer
        {
            get { return _useChunkedTransfer; }
        }

        public float TimeOutSeconds
        {
            get { return _timeOutSeconds; }
        }

        public int RetryCount
        {
            get { return _retryCount; }
        }
    }
}
