using System;
using UnityEngine;

namespace SocialGame.Internal.Network.HTTP
{
    [Serializable]
    internal sealed class HttpSettings
    {
        public enum Format
        {
            JSON,
            MsgPack,
            XML,
        }
        
        [SerializeField] private string _domain = "https://www.google.co.jp/";

        [SerializeField] private string _productionEnvironment = "";

        [SerializeField] private string _stagingEnvironment = "stg";

        [SerializeField] private string _developmentEnvironment = "dev";
        
        [SerializeField] private Format _dataFormat = Format.JSON;
        
        [SerializeField] private bool _useChunkedTransfer = true;

        public string Domain => _domain;

        public string ProductionEnvironment => _productionEnvironment;

        public string StagingEnvironment => _stagingEnvironment;

        public string DevelopmentEnvironment => _developmentEnvironment;

        public Format DataFormat => _dataFormat;

        public bool UseChunkedTransfer => _useChunkedTransfer;
    }
}
