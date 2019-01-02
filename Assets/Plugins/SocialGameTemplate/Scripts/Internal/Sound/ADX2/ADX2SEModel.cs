#if SGT_ADX2
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
using UnityObject = UnityEngine.Object;

namespace SocialGame.Internal.Sound.ADX2
{
    internal sealed class ADX2SEModel : IInitializable, IDisposable, ISoundModel
    {
        [Inject] private ISEIntent _intent = null;

        [Inject] private ISoundVolumeIntent _volumeIntent = null;

        [Inject] private ADX2SESettings _settings = null;
        
        private readonly BoolReactiveProperty _initialized = new BoolReactiveProperty();

        private readonly FloatReactiveProperty _masterVolume = new FloatReactiveProperty(1.0f);
        
        private readonly FloatReactiveProperty _volume = new FloatReactiveProperty(1.0f);

        private readonly ReactiveProperty<CriAtomSource> _source = new ReactiveProperty<CriAtomSource>();
        
        private readonly Dictionary<string, string> _cueSheetDictionary = new Dictionary<string, string>();

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        void IInitializable.Initialize()
        {
            Observable.EveryUpdate()
                .Where(_ => CriWareInitializer.IsInitialized())
                .First()
                .SelectMany(_ => ADX2Utility.AddCueSheet(_settings.BuiltInCueSheet)
                    .First())
                .Subscribe(cueSheet =>
                {
                    if (cueSheet != null)
                    {
                        // add cue list
                        var cueNameList = ADX2Utility.GetCueNameList(cueSheet.name);
                        foreach (var cueName in cueNameList)
                            _cueSheetDictionary.Add(cueName, cueSheet.name);

                        // create sound source
                        var source = new GameObject("SE").AddComponent<CriAtomSource>();
                        _source.Value = source;
                    }

                    _initialized.Value = true;
                })
                .AddTo(_disposable);

            Observable.Merge(
                    _intent.OnPlayAsObservable().Select(x => x.ToString()),
                    _intent.OnPlayForNameAsObservable())
                .Where(_ => _initialized.Value && _source.Value != null)
                .Subscribe(x =>
                {
                    string cueSheet;
                    if (!_cueSheetDictionary.TryGetValue(x, out cueSheet))
                    {
                        Debug.unityLogger.LogError(GetType().Name, $"{x} is not found.");
                        return;
                    }

                    _source.Value.cueSheet = cueSheet;
                    _source.Value.Play(x);
                })
                .AddTo(_disposable);
            
            // volume
            _volumeIntent.OnMasterVolumeAsObservable()
                .Subscribe(x => _masterVolume.Value = x)
                .AddTo(_disposable);
            
            _volumeIntent.OnVoiceVolumeAsObservable()
                .Subscribe(x => _volume.Value = x)
                .AddTo(_disposable);

            var initialize = _initialized
                .Where(x => x)
                .First()
                .Publish()
                .RefCount();
            
            
            initialize
                .Where(_ => _source.Value != null)
                .SelectMany(_ => _masterVolume)
                .Subscribe(x => _source.Value.volume = x)
                .AddTo(_disposable);
            
            initialize
                .SelectMany(_ => _volume)
                .Subscribe(x => CriAtom.SetCategoryVolume(_settings.CategoryVolumeName, x))
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }
        
        #region ISoundModel implementation
        IObservable<Unit> ISoundModel.OnInitializedAsObservable()
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
