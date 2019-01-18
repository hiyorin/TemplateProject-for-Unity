using System;
using UniRx;
using UniRx.Async;

namespace SocialGame.Scene
{
    public interface ISceneLifecycle
    {
        UniTask OnLoad(object transData);
        
        UniTask OnTransIn();

        void OnTransComplete();

        UniTask OnTransOut();

        UniTask OnUnload();
    }
}
