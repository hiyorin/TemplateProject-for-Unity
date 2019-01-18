using SocialGame.Data;
using SocialGame.Internal.Data.Entity;
using Zenject;
using UniRx.Async;

namespace SocialGame.Internal.Data.DataStore
{
    internal sealed class ResolutionLocalDataStore : IResolutionDataStore
    {
        [Inject] private ResolutionLocalStorage _storage = null;
    
        #region IResolutionDataStore implementation
        async UniTask<Resolution> IResolutionDataStore.Get()
        {
            return _storage.Model;
        }
    
        async UniTask IResolutionDataStore.Put(Quality quality)
        {
            _storage.Model.Quality = quality;
            await _storage.SaveAsync();
        }
        #endregion
    }
}