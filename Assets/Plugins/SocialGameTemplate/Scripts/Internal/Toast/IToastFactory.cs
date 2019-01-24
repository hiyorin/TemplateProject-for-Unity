using UnityEngine;
using UniRx.Async;

namespace SocialGame.Internal.Toast
{
    internal interface IToastFactory
    {
        UniTask<GameObject> Create(string name);
    }
}
