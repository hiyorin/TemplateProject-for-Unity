using System;
using UnityEngine;
using Zenject;
using UnityObject = UnityEngine.Object;

namespace SocialGame.Internal.Sound.ADX2
{
    public sealed class ADX2MasterModel : IInitializable
    {
        [Inject] private ADX2MasterSettings _settings = null;
        
        void IInitializable.Initialize()
        {
            UnityObject.Instantiate(_settings.InitializerPrefab);
        }
    }
}
