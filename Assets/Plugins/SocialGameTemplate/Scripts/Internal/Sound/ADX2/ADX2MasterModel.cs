#if SGT_ADX2
using System;
using Zenject;
using UniRx;
using UnityObject = UnityEngine.Object;

namespace SocialGame.Internal.Sound.ADX2
{
    internal sealed class ADX2MasterModel : IInitializable, IDisposable
    {
        [Inject] private ADX2MasterSettings _settings = null;
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        void IInitializable.Initialize()
        {
            var initializer = UnityObject.Instantiate(_settings.InitializerPrefab);
            if (initializer.gameObject.GetComponent<CriAtom>() == null)
            {
                var atom = initializer.gameObject.AddComponent<CriAtom>();
                atom.dontDestroyOnLoad = initializer.dontDestroyOnLoad;
                atom.acfFile = initializer.atomConfig.acfFileName;
            }
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }
    }
}
#endif
