using System;
using System.Linq;
using SocialGame.Sound;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using UniRx;

namespace SocialGame.Internal
{
    public sealed class ProjectModel : IInitializable, IDisposable, IProject
    {
        [Inject] private ISoundController _soundController = null;
        
        [Inject] private ApplicationSettings _settings = null;

        private readonly BoolReactiveProperty _initialized = new BoolReactiveProperty();

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public static readonly string[] SystemSceneNames = {
            "Transition",
            "Dialog",
            "Toast",
            "TapEffect",
            "Loading",
        };

        public const string RootPath = "Plugins/SocialGameTemplate";

        void IInitializable.Initialize()
        {
            SystemSceneNames
                .Select(x => SceneManager.LoadSceneAsync(x, LoadSceneMode.Additive).AsObservable().AsUnitObservable())
                .WhenAll()
                // wait, initialize scene context
                .SelectMany(_ => Observable.NextFrame())
                // wait, initialize module
                .SelectMany(_ => _soundController.OnInitializedAsObservable())
                .Subscribe(_ => _initialized.Value = true)
                .AddTo(_disposable);

            Application.targetFrameRate = _settings.TargetFrameRate;
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }

        #region IProject implementation
        IObservable<Unit> IProject.OnInitializedAsObservable()
        {
            return _initialized
                .Where(x => x)
                .AsUnitObservable();
        }
        #endregion
    }
}
