using UnityEngine;
using UniRx.Async;

namespace SocialGame.Internal.Dialog
{
    internal interface IDialogFactory
    {
        UniTask<GameObject> Spawn(string name);

        void Despawn(string name, GameObject value);
    }
}
