using UnityEngine;
using UniRx.Async;

namespace SocialGame.TapEffect
{
    public interface ITapEffect
    {
        void OnMove(Vector3 position);

        UniTask OnShow();

        UniTask OnHide();
    }
}
