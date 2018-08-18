using System;
using System.Collections.Generic;
using SocialGame.Toast;
using UnityEngine;
using Zenject;
using UniRx;

namespace SocialGame.Internal.Toast
{
    internal sealed class ToastModel : IInitializable, IDisposable, IToastModel
    {
        private class Context
        {
            public IToast Toast;
            public GameObject Object;
        }

        [Serializable]
        private class Request
        {
            public IToast Toast;
            public object Param;
        }

        [Inject] private IToastIntent _intent = null;

        [Inject] private IToastFactory _factory = null;

        [Inject] private ToastSettings _settings = null;

        private readonly ReactiveProperty<Request> _current = new ReactiveProperty<Request>();

        private readonly Queue<Request> _requests = new Queue<Request>();

        private readonly ReactiveDictionary<ToastType, Context> _contexts = new ReactiveDictionary<ToastType, Context>();

        private readonly BoolReactiveProperty _isOpen = new BoolReactiveProperty();

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        void IInitializable.Initialize()
        {
            _intent
                .OnOpenAsObservable()
                .Select(request => {
                    Context context = null;
                    if (!_contexts.TryGetValue(request.Type, out context))
                    {
                        var toastObject = _factory.Create(request.Type);
                        context = new Context() {
                            Toast = toastObject.GetComponent<IToast>(),
                            Object = toastObject,
                        };
                        _contexts.Add(request.Type, context);
                    }
                    return new Request() {
                        Toast = context.Toast,
                        Param = request.Param,
                    };
                })
                .Where(x => x.Toast != null)
                .Subscribe(x => {
                    if (_current.Value == null)
                    {
                        _isOpen.Value = true;
                        _current.Value = x;
                    }
                    else
                        _requests.Enqueue(x);
                })
                .AddTo(_disposable);

            Observable
                .EveryUpdate()
                .Where(_ => _current.Value == null)
                .Select(_ => _requests.Count > 0)
                .Do(x => _isOpen.Value = x)
                .Where(x => x)
                .Select(_ => _requests.Dequeue())
                .Subscribe(x => _current.Value = x)
                .AddTo(_disposable);

            _current
                .Where(x => x != null)
                .SelectMany(x => x.Toast.OnOpenAsObservable(x.Param)
                    .First()
                    .Select(_ => x))
                .SelectMany(x => Observable.Timer(TimeSpan.FromSeconds(_settings.ShowDuration))
                    .First()
                    .Select(_ => x))
                .SelectMany(x => x.Toast.OnCloseAsObservable())
                .Subscribe(_ => _current.Value = null)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }

        #region IToastModel implementation
        IObservable<GameObject> IToastModel.OnAddAsObservable()
        {
            return _contexts
                .ObserveAdd()
                .Select(x => x.Value.Object);
        }

        IObservable<Unit> IToastModel.OnOpenAsObservable()
        {
            return _isOpen
                .Where(x => x)
                .AsUnitObservable();
        }

        IObservable<Unit> IToastModel.OnCloseAsObservable()
        {
            return _isOpen
                .Where(x => !x)
                .AsUnitObservable();
        }
        #endregion
    }
}
