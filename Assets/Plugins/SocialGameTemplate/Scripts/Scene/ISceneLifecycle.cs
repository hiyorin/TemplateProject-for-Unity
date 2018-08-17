using System;
using UniRx;

namespace SocialGame.Scene
{
    public interface ISceneLifecycle
    {
        IObservable<Unit> OnLoad(object transData);

        IObservable<Unit> OnTransIn();

        void OnTransComplete();

        IObservable<Unit> OnTransOut();

        IObservable<Unit> OnUnload();
    }
}
