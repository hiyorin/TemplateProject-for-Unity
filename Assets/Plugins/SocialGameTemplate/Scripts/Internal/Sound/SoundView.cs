using System;
using System.Linq;
using UnityEngine;
using Zenject;
using UniRx;

namespace SocialGame.Internal.Sound
{
    internal sealed class SoundView : MonoBehaviour
    {
        [Inject] private ISoundModel[] _models = null;

        private void Start()
        {
            _models
                .Select(x => x.OnAddObjectAsObservable())
                .Merge()
                .Subscribe(x => x.parent = transform)
                .AddTo(this);
        }
    }
}
