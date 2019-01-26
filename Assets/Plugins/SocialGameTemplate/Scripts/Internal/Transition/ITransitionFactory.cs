using UnityEngine;
using UniRx.Async;

namespace SocialGame.Internal.Transition
{
    internal interface ITransitionFactory
    {
        UniTask<GameObject> Create(string name);
    }
}
