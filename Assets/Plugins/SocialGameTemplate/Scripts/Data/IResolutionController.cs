using System;
using UniRx;

namespace SocialGame.Data
{    
    [Serializable]
    public enum Quality
    {
        Low,
        Standard,
        High,
    }

    public interface IResolutionController
    {
        IObservable<Unit> Put(Quality quality);

        IObservable<Quality> Get();
    }
}
