using System;
using UnityEngine;
using Zenject;
using UniRx;

namespace SocialGame.Internal.Sound.ADX2
{
    internal sealed class ADX2VoiceModel : IInitializable, IDisposable, ISoundModel
    {
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        void IInitializable.Initialize()
        {
            
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }

        #region ISoundModel implementation
        IObservable<Unit> ISoundModel.OnInitializeAsObservable()
        {
            return Observable.ReturnUnit();
        }
        
        IObservable<Transform> ISoundModel.OnAddObjectAsObservable()
        {
            return Observable.Empty<Transform>();
        }
        #endregion
    }
}