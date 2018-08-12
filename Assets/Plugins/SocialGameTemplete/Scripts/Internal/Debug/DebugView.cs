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

        [SerializeField] private Text _memory = null;

        [Inject] private IFPSModel _fpsModel = null;

        [Inject] private IMemoryModel _memoryModel = null;

        [Inject] private DebugSettings _settings = null;

        private void Start()
        {
            _fps.text = string.Empty;
            _fps.color = _settings.TextColor;

            _memory.text = string.Empty;
            _memory.color = _settings.TextColor;

            gameObject.SetActiveSafe(_settings.FPS || _settings.Memory);

            _fpsModel
                .OnUpdateFPSAsObservable()
                .Subscribe(x => _fps.text = string.Format("FPS : {0:F2}", x))
                .AddTo(this);

            _memoryModel
                .OnUpdateMomoryInfoAsObservable()
                .Subscribe(x => {
                    float usedSizeMB = x.UsedSize / 1024.0f;
                    float totalSizeMB = x.TotalSize / 1024.0f;
                    _memory.text = string.Format("Memory : {0:0.00}/{1:0.00} MB ({2:0.0}%)", usedSizeMB, totalSizeMB, 100.0f * usedSizeMB / totalSizeMB);
                })
                .AddTo(this);
        }

    }
}
