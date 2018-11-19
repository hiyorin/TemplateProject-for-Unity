using System;
using SocialGame.Data;
using SocialGame.Internal.Data.Entity;
using UniRx;

namespace SocialGame.Internal.Data.DataStore
{
    internal interface IResolutionDataStore
    {
        IObservable<Resolution> Get();

        IObservable<Unit> Put(Quality quality);
    }
}
