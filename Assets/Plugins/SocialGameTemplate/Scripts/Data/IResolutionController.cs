using System;
using UniRx;

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
        IObservable<Quality> Put(Quality quality);

        IObservable<Quality> Get();
    }
}
