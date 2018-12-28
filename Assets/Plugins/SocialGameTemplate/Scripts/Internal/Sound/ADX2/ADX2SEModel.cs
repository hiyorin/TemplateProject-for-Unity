#if SGT_ADX2
using System;
using UnityEngine;
using Zenject;
using UniRx;
using UnityObject = UnityEngine.Object;

namespace SocialGame.Internal.Sound.ADX2
{
    internal sealed class ADX2SEModel : IInitializable, IDisposable, ISoundModel
    {
        [Inject] private ADX2SESettings _settings = null;
        
        private readonly ReactiveProperty<CriAtomSource> _source = new ReactiveProperty<CriAtomSource>();
        
        private readonly BoolReactiveProperty _initialized = new BoolReactiveProperty();

        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        void IInitializable.Initialize()
        {
            Observable.EveryUpdate()
                .Where(_ => CriWareInitializer.IsInitialized())
                .First()
                .SelectMany(_ => ADX2Utility.AddCueSheet(_settings.BuiltInCueSheet).First())
                .Subscribe(_ =>
                {
                    var source = new GameObject("SE").AddComponent<CriAtomSource>();

                    _source.Value = source;
                })
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            ADX2Utility.RemoveCueSheet(_settings.BuiltInCueSheet);
            
            if (_source.Value != null)
            {
                UnityObject.Destroy(_source.Value.gameObject);
                _source.Value = null;
            }
            
            _disposable.Dispose();
        }
        
        #region ISoundModel implementation
        IObservable<Unit> ISoundModel.OnInitializeAsObservable()
        {
            return _initialized
                .Where(x => x)
                .AsUnitObservable();
        }
        
        IObservable<Transform> ISoundModel.OnAddObjectAsObservable()
        {
            return _source
                .Where(x => x != null)
                .Select(x => x.transform);
        }
        #endregion
    }
}
#endif
