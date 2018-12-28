using System;
using UnityEngine;
using Zenject;
using UniRx;

namespace SocialGame.Internal.Sound.ADX2
{
    internal sealed class ADX2BGMModel : IInitializable, IDisposable, IBGMModel
    {
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        void IInitializable.Initialize()
        {
            
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }
        
        #region IBGMModel implementation
        IObservable<Transform> IBGMModel.OnAddObjectAsObservable()
        {
            return Observable.Empty<Transform>();
        }
        #endregion
    }
}