using System;
using UniRx;

namespace SocialGame.Internal.DebugMode
{
    internal interface IExtensionIntent
    {
        IObservable<Object> OnAddObservable();

        IObservable<Object> OnRemoveObervable();
    }
}
