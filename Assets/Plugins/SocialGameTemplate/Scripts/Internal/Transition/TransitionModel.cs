using System;
using System.Collections.Generic;
using SocialGame.Transition;
using UnityEngine;
using Zenject;
using UniRx;

namespace SocialGame.Internal.Transition
{
    internal sealed class TransitionModel : IInitializable, IDisposable, ITransitionModel
    {
        [Inject] private ITransitionFactory _factory = null;

        [Inject] private ITransitionIntent _intent = null;
        
        private readonly Stack<ITransition> _transStack = new Stack<ITransition>();

        private readonly ReactiveDictionary<TransMode, GameObject> _transObjects = new ReactiveDictionary<TransMode, GameObject>();

        private readonly Subject<Unit> _onTransInComplete = new Subject<Unit>();

        private readonly Subject<Unit> _onTransOutComplete = new Subject<Unit>();

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        void IInitializable.Initialize()
        {
            _intent.OnTransInAsObservable()
                .Where(x => x != TransMode.None)
                .Select(trans => {
                    GameObject transObject = null;
                    if (!_transObjects.TryGetValue(trans, out transObject))
                    {
                        transObject = _factory.Create(trans);
                        _transObjects.Add(trans, transObject);
                    }
                    return transObject;
                })
                .Select(x => x.GetComponent<ITransition>())
                .Where(x => x != null)
                .Do(x => _transStack.Push(x))
                .SelectMany(x => x.OnTransInAsObservable().First())
                .Subscribe(_onTransInComplete.OnNext)
                .AddTo(_disposable);

            _intent.OnTransOutAsObservable()
                .Where(_ => _transStack.Count > 0)
                .Select(_ => _transStack.Pop())
                .SelectMany(x => x.OnTransOutAsObservable().First())
                .Subscribe(_onTransOutComplete.OnNext)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
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
