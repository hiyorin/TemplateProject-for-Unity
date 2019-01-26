using UnityEngine;
using UniRx.Async;

namespace SocialGame.Internal.TapEffect
{
    internal interface ITapEffectFactory
    {
        UniTask<GameObject> Create(string name);
    }
}
