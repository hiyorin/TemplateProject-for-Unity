using System;
using System.Linq;
using UnityEngine.SceneManagement;
using Zenject;
using UniRx;

namespace SocialGame.Internal
{
    public sealed class ProjectModel : IInitializable, IDisposable
    {
        private readonly BoolReactiveProperty _initialized = new BoolReactiveProperty();

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public readonly static string[] SystemSceneNames = {
            "Transition",
            "Dialog",
            "Toast",
            "TapEffect",
            "Loading",
        };

        public const string RootPath = "Plugins/SocialGameTemplete";

        void IInitializable.Initialize()
        {
            SystemSceneNames
                .Select(x => SceneManager.LoadSceneAsync(x, LoadSceneMode.Additive).AsObservable().AsUnitObservable())
                .WhenAll()
                .Subscribe(_ => _initialized.Value = true)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }

        public IObservable<Unit> OnInitializedAsObservable()
        {
            return _initialized
                .Where(x => x)
                .AsUnitObservable();
        }
    }
}
