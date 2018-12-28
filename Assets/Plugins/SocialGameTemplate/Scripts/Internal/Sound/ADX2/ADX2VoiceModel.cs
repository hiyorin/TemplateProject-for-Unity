using System;
using UnityEngine;
using Zenject;
using UniRx;

namespace SocialGame.Internal.Sound.ADX2
{
    internal sealed class ADX2VoiceModel : IInitializable, IDisposable, IVoiceModel
    {
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        void IInitializable.Initialize()
        {
            
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }

        #region IVoiceModel implementation
        IObservable<Transform> IVoiceModel.OnAddObjectAsObservable()
        {
            return Observable.Empty<Transform>();
        }
        #endregion
    }
}