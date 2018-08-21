using System;
using UnityEngine;
using UnityEngine.UI;
using UnityExtensions;
using Zenject;
using UniRx;

namespace SocialGame.Internal.DebugMode
{
    internal sealed class DebugView : MonoBehaviour
    {
        [SerializeField] private Text _fps = null;

        [SerializeField] private Text _memory = null;

        [SerializeField] private Text _extension = null;

        [Inject] private IFPSModel _fpsModel = null;

        [Inject] private IMemoryModel _memoryModel = null;

        [Inject] private IExtensionModel _extensionModel = null;

        [Inject] private DebugSettings _settings = null;

        private void Start()
        {
            Reset(_fps);
            Reset(_memory);
            Reset(_extension);

            gameObject.SetActiveSafe(_settings.FPS || _settings.Memory || _settings.Extension);
            if (!gameObject.activeSelf)
                return;
            
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

            _extensionModel
                .OnUpdateExtensionAsObservable()
                .Subscribe(x => _extension.text = x)
                .AddTo(this);
        }

        private void Reset(Text text)
        {
            text.text = string.Empty;
            text.color = _settings.TextColor;
            text.fontSize = _settings.TextFontSize;
        }
    }
}
