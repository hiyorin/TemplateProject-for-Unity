using System;
using SocialGame.Loading;
using UnityEngine;
using Zenject;
using UniRx;
using UniRx.Async;

namespace SocialGame.Internal.Loading
{
    internal sealed class LoadingModel : IInitializable, IDisposable, ILoadingModel
    {
        private class Context
        {
            public ILoading Loading;
            public GameObject Object;
        }

        [Inject] private ILoadingIntent _intent = null;

        [Inject] private ILoadingFactory _factory = null;

        [Inject] private LoadingSettings _settigns = null;

        private readonly ReactiveDictionary<string, Context> _contexts = new ReactiveDictionary<string, Context>();

        private readonly BoolReactiveProperty _isShow = new BoolReactiveProperty();

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        void IInitializable.Initialize()
        {
            _intent.OnShowAsObservable()
                .Subscribe(x => Process(x).GetAwaiter())
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }

        private async UniTask Process(string name)
        {
            if (_isShow.Value)
                return;

            // start
            _isShow.Value = true;

            var loading = await Find(name);
            await loading.OnShow(_settigns.DefaultDuration);
            await _intent.OnHideAsObservable().First();
            await loading.OnHide(_settigns.DefaultDuration);

            // complete
            _isShow.Value = false;
        }

        private async UniTask<ILoading> Find(string name)
        {
            Context context = null;
            if (!_contexts.TryGetValue(name, out context))
            {
                var loadingObject = await _factory.Create(name);
                context = new Context() {
                    Loading = loadingObject.GetComponent<ILoading>(),
                    Object = loadingObject,
                };
                _contexts.Add(name, context);
            }
            return context.Loading;
        }
        
        #region ILoadingModel implementation
        IObservable<GameObject> ILoadingModel.OnAddAsObservable()
        {
            return _contexts
                .ObserveAdd()
                .Select(x => x.Value.Object);
        }

        IObservable<Unit> ILoadingModel.OnShowAsObservable()
        {
            return _isShow
                .Where(x => x)
                .AsUnitObservable();
        }

        IObservable<Unit> ILoadingModel.OnHideAsObservable()
        {
            return _isShow
                .Where(x => !x)
                .AsUnitObservable();
        }
        #endregion
    }
}