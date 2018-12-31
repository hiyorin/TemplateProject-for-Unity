using System;
using SocialGame.Internal;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SocialGame
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasScaler))]
    public sealed class CanvasSettings : MonoBehaviour
    {
        [Serializable]
        private enum Layer
        {
            Background  = 0,
            Foreground  = 100,
            Dialog      = 600,
            Toast       = 700,
            Transition  = 800,
            Loading     = 900,
            TapEffect   = 1000,
        }

        [SerializeField] private Layer _layer = Layer.Foreground;

        [SerializeField] [Range(0, 99)] private int _order = 0;

        [Inject] private Camera _uiCamera = null;

        [Inject] private ResolutionSettings _resolutionSettings = null;
        
        private void Start()
        {
            var canvas = GetComponent<Canvas>();
            canvas.worldCamera = _uiCamera;
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.sortingOrder = (int)_layer + _order;

            InitializeCanvasScaler();
        }

        private void InitializeCanvasScaler()
        {
            var canvsScaler = GetComponent<CanvasScaler>();
            canvsScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvsScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            canvsScaler.referenceResolution = _resolutionSettings.CanvasSize;
        }

        #if UNITY_EDITOR
        private void OnValidate()
        {
            var canvas = GetComponent<Canvas>();
            canvas.sortingOrder = (int)_layer;

            _resolutionSettings = Resources.Load<ResolutionSettings>(ResolutionSettings.FileName);
            InitializeCanvasScaler();
        }
        #endif
    }
}
