using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SocialGame.Examples.Sound
{
    internal sealed class SoundExampleView : MonoBehaviour
    {
        [SerializeField] private Dropdown _bgmDropdown = null;

        [Inject] private ISoundExampleModel _model = null;
        
        private void Start()
        {
            _bgmDropdown.ClearOptions();
            
            _model.OnAddBgmAsObservable()
                .Select(x => new List<string>(x))
                .Subscribe(x => _bgmDropdown.AddOptions(x))
                .AddTo(this);
        }
    }
}