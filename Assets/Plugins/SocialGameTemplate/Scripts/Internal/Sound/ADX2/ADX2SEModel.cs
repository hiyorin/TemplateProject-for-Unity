using System;
using UnityEngine;
using Zenject;
using UniRx;

namespace SocialGame.Internal.Sound.ADX2
{
    internal sealed class ADX2SEModel : IInitializable, IDisposable, ISEModel
    {
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        void IInitializable.Initialize()
        {
            
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }
        
        #region ISEModel implementation
        IObservable<Transform> ISEModel.OnAddObjectAsObservable()
        {
            return Observable.Empty<Transform>();
        }
        #endregion
    }
}