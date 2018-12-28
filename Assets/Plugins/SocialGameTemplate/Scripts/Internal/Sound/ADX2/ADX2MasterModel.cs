#if SGT_ADX2
using System;
using Zenject;
using UniRx;
using UnityObject = UnityEngine.Object;

namespace SocialGame.Internal.Sound.ADX2
{
    public sealed class ADX2MasterModel : IInitializable, IDisposable
    {
        [Inject] private ADX2MasterSettings _settings = null;
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        void IInitializable.Initialize()
        {
            UnityObject.Instantiate(_settings.InitializerPrefab);
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }
    }
}
#endif
