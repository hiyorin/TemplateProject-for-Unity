using SocialGame.Data;
using SocialGame.Internal.Data.Entity;
using UniRx.Async;

namespace SocialGame.Internal.Data.DataStore
{
    internal interface IResolutionDataStore
    {
        UniTask<Resolution> Get();

        UniTask Put(Quality quality);
    }
}
