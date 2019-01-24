using UnityEngine;
using UniRx.Async;

namespace SocialGame.Internal.Loading
{
    internal interface ILoadingFactory
    {
        UniTask<GameObject> Create(string name);
    }
}
