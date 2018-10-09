using System;
using UnityEngine;
using Zenject;

namespace SocialGame
{
    [RequireComponent(typeof(Canvas))]
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

        private void Start()
        {
            var canvas = GetComponent<Canvas>();
            canvas.worldCamera = _uiCamera;
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.sortingOrder = (int)_layer + _order;
        }

        #if UNITY_EDITOR
        private void OnValidate()
        {
            var canvas = GetComponent<Canvas>();
            canvas.sortingOrder = (int)_layer;
        }
        #endif
    }
}
