using System;
using SocialGame.Data;
using SocialGame.Internal.Data.Entity;
using Zenject;
using UniRx;

namespace SocialGame.Internal.Data.DataStore
{
    internal sealed class ResolutionLocalDataStore : IResolutionDataStore
    {
        [Inject] private ResolutionLocalStorage _storage;
    
        #region IResolutionDataStore implementation
        IObservable<Resolution> IResolutionDataStore.Get()
        {
            return Observable.Return(_storage.Model);
        }
    
        IObservable<Unit> IResolutionDataStore.Put(Quality quality)
        {
            _storage.Model.Quality = quality;
            return _storage.SaveAsync();
        }
        #endregion
    }
}