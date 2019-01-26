using System;
using SocialGame.TapEffect;
using UnityEngine;
using Zenject;
using UniRx;
using UniRx.Async;

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

        [Inject] private ITapEffectFactory _factory = null;

        private readonly ReactiveProperty<Context> _currentContext = new ReactiveProperty<Context>();

        private readonly ReactiveDictionary<string, Context> _contexts = new ReactiveDictionary<string, Context>();

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        void IInitializable.Initialize()
        {
            _intent.OnStartAsObservable()
                .Subscribe(x => Start(x).GetAwaiter())
                .AddTo(_disposable);

            _intent.OnStopAsObservable()
                .Where(_ => _currentContext.Value != null)
                .SelectMany(_ => _currentContext.Value.TapEffect.OnHide().ToObservable())
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
                .SelectMany(x => x.OnShow().ToObservable())
                .Subscribe()
                .AddTo(_disposable);

            current
                .Where(_ => Input.GetMouseButtonUp(0))
                .SelectMany(x => x.OnHide().ToObservable())
                .Subscribe()
                .AddTo(_disposable);

            current
                .Where(_ => Input.GetMouseButton(0))
                .Subscribe(x => x.OnMove(Input.mousePosition))
                .AddTo(_disposable);
        }

        private async UniTask Start(string name)
        {
            Context context = null;
            if (!_contexts.TryGetValue(name, out context))
            {
                var tapEffectObject = await _factory.Create(name);
                context = new Context() {
                    TapEffect = tapEffectObject.GetComponent<ITapEffect>(),
                    Object = tapEffectObject,
                };
                _contexts.Add(name, context);
            }

            _currentContext.Value = context;
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
