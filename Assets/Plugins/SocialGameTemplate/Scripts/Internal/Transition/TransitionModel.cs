using System;
using System.Collections.Generic;
using SocialGame.Transition;
using UnityEngine;
using Zenject;
using UniRx;
using UniRx.Async;

namespace SocialGame.Internal.Transition
{
    internal sealed class TransitionModel : IInitializable, IDisposable, ITransitionModel
    {
        [Inject] private ITransitionFactory _factory = null;

        [Inject] private ITransitionIntent _intent = null;

        [Inject] private TransitionSettings _settings = null;
        
        private readonly Stack<ITransition> _transStack = new Stack<ITransition>();

        private readonly ReactiveDictionary<string, GameObject> _transObjects = new ReactiveDictionary<string, GameObject>();

        private readonly Subject<Unit> _onTransInComplete = new Subject<Unit>();

        private readonly Subject<Unit> _onTransOutComplete = new Subject<Unit>();

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        void IInitializable.Initialize()
        {
            _intent.OnTransInAsObservable()
                .Where(x => !string.IsNullOrEmpty(x))
                .Subscribe(x => TransIn(x).GetAwaiter())
                .AddTo(_disposable);

            _intent.OnTransOutAsObservable()
                .Subscribe(_ => TransOut().GetAwaiter())
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }

        async UniTask TransIn(string name)
        {
            GameObject transObject = null;
            if (!_transObjects.TryGetValue(name, out transObject))
            {
                transObject = await _factory.Create(name);
                _transObjects.Add(name, transObject);
            }

            if (transObject == null)
            {
                Debug.unityLogger.LogError(GetType().Name, $"{name} is not found.");
                return;
            }

            var transition = transObject.GetComponent<ITransition>();
            if (transition == null)
            {
                Debug.unityLogger.LogError(GetType().Name, $"{name} is not implementation.");
                return;
            }

            _transStack.Push(transition);
            await transition.OnTransIn(_settings.DefaultDuration);
            _onTransInComplete.OnNext(Unit.Default);
        }

        async UniTask TransOut()
        {
            var transition = _transStack.Pop();
            await transition.OnTransOut(_settings.DefaultDuration);
            _onTransOutComplete.OnNext(Unit.Default);
        }
        
        #region ITransitionModel implementation
        IObservable<GameObject> ITransitionModel.OnAddAsObservable()
        {
            return _transObjects
                .ObserveAdd()
                .Select(x => x.Value);
        }

        IObservable<Unit> ITransitionModel.OnTransInCompleteAsObservable()
        {
            return _onTransInComplete;
        }

        IObservable<Unit> ITransitionModel.OnTransOutCompleteAsObservable()
        {
            return _onTransOutComplete;
        }
        #endregion
    }
}
