using System;
using SocialGame.TapEffect;
using UnityEngine;
using Zenject;
using UniRx;

namespace SocialGame.Internal.TapEffect
{
    internal sealed class TapEffectModel : IInitializable, IDisposable, ITapEffectModel
    {
        private class Context
        {
            public ITapEffect TapEffect;
            public GameObject Object;
        }

        [Inject] private ITapEffectIntent _intent = null;

        [Inject] private TapEffectFactory _factory = null;

        private readonly ReactiveProperty<Context> _currentContext = new ReactiveProperty<Context>();

        private readonly ReactiveDictionary<TapEffectType, Context> _contexts = new ReactiveDictionary<TapEffectType, Context>();

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        void IInitializable.Initialize()
        {
            _intent
                .OnStartAsObservable()
                .Select(x => {
                    Context context = null;
                    if (!_contexts.TryGetValue(x, out context))
                    {
                        var tapEffectObject = _factory.Create(x);
                        context = new Context() {
                            TapEffect = tapEffectObject.GetComponent<ITapEffect>(),
                            Object = tapEffectObject,
                        };
                        _contexts.Add(x, context);
                    }
                    return context;
                })
                .Subscribe(x => _currentContext.Value = x)
                .AddTo(_disposable);

            _intent
                .OnStopAsObservable()
                .Subscribe(_ => _currentContext.Value = null)
                .AddTo(_disposable);

            var current = Observable.EveryUpdate()
                .Select(_ => _currentContext.Value)
                .Where(x => x != null)
                .Select(x => x.TapEffect)
                .Publish()
                .RefCount();

            current
                .Where(_ => Input.GetMouseButtonDown(0))
                .SelectMany(x => x.OnShowAsObservable().First())
                .Subscribe()
                .AddTo(_disposable);

            current
                .Where(_ => Input.GetMouseButtonUp(0))
                .SelectMany(x => x.OnHideAsObservable().First())
                .Subscribe()
                .AddTo(_disposable);

            current
                .Where(_ => Input.GetMouseButton(0))
                .Subscribe(x => x.OnMove(Input.mousePosition))
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }

        #region ITapEffectModel implementation
        IObservable<GameObject> ITapEffectModel.OnAddAsObservable()
        {
            return _contexts
                .ObserveAdd()
                .Select(x => x.Value.Object);
        }
        #endregion
    }
}
