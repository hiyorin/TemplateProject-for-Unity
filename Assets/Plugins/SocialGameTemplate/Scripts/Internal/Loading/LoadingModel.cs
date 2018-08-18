using System;
using SocialGame.Loading;
using UnityEngine;
using Zenject;
using UniRx;

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

        private readonly ReactiveDictionary<LoadingType, Context> _contexts = new ReactiveDictionary<LoadingType, Context>();

        private readonly BoolReactiveProperty _isShow = new BoolReactiveProperty();

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        void IInitializable.Initialize()
        {
            _intent
                .OnShowAsObservable()
                .Where(_ => !_isShow.Value)
                .Do(_ => _isShow.Value = true)
                .Select(type => {
                    Context context = null;
                    if (!_contexts.TryGetValue(type, out context))
                    {
                        var loadingObject =_factory.Create(type);
                        context = new Context() {
                            Loading = loadingObject.GetComponent<ILoading>(),
                            Object = loadingObject,
                        };
                        _contexts.Add(type, context);
                    }
                    return context.Loading;
                })
                .SelectMany(x => x.OnShowAsObservable().First().Select(_ => x))
                .SelectMany(x => _intent.OnHideAsObservable().First().Select(_ => x))
                .SelectMany(x => x.OnHideAsObservable().First())
                .Do(_ => _isShow.Value = false)
                .Subscribe()
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
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