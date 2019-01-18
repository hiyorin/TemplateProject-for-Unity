using System;
using UniRx.Async;

namespace SocialGame.Data
{    
    [Serializable]
    public enum Quality
    {
        Low,
        Middle,
        High,
    }

    public interface IResolutionController
    {
        UniTask<Quality> Put(Quality quality);

        UniTask<Quality> Get();
    }
}
