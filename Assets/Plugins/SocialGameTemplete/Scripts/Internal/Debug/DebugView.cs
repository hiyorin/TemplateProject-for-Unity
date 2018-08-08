using System;
using UnityEngine;
using UnityEngine.UI;
using UnityExtensions;
using Zenject;
using UniRx;

namespace SocialGame.Internal.DebugMode
{
    public sealed class DebugView : MonoBehaviour
    {
        [SerializeField] private Text _fps = null;

        [Inject] private IFPSModel _fpsModel = null;

        private void Start()
        {
            _fps.text = string.Empty;
            gameObject.SetActiveSafe(false);

            _fpsModel
                .OnUpdateFPSAsObservable()
                .Do(_ => gameObject.SetActiveSafe(true))
                .Subscribe(x => _fps.text = string.Format("FPS : {0:F2}", x))
                .AddTo(this);
        }

    }
}
