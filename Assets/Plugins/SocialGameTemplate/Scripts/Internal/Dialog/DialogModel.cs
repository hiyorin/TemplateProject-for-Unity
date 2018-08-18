using System;
using System.Collections.Generic;
using SocialGame.Dialog;
using UnityEngine;
using Zenject;
using UniRx;

namespace SocialGame.Internal.Dialog
{
    internal sealed class DialogModel : IInitializable, IDisposable, IDialogModel
    {
        private class Context
        {
            public IDialog Dialog;
            public GameObject Object;
        }

        private class Request
        {
            public IDialog Dialog;
            public object Param;
        }

        [Inject] private IDialogIntent _intent = null;

        [Inject] private IDialogFactory _factory = null;

        private readonly Stack<Request> _stack = new Stack<Request>();

        private readonly ReactiveDictionary<DialogType, Context> _contexts = new ReactiveDictionary<DialogType, Context>();

        private readonly BoolReactiveProperty _isOpen = new BoolReactiveProperty();

        private readonly Subject<RequestDialog> _onOpen = new Subject<RequestDialog>();

        private readonly Subject<object> _onClose = new Subject<object>();

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        void IInitializable.Initialize()
        {
            _contexts
                .ObserveAdd()
                .Select(x => x.Value.Dialog)
                .Subscribe(x => {
                    x.OnNextAsObservable()
                        .Subscribe(y => _onOpen.OnNext(y))
                        .AddTo(_disposable);

                    x.OnPreviousAsObservable()
                        .Subscribe(y => _onClose.OnNext(y))
                        .AddTo(_disposable);
                })
                .AddTo(_disposable);

            Observable
                .Merge(_intent.OnOpenAsObservable().Where(_ => !_isOpen.Value), _onOpen)
                .Select(request => {
                    Context context = null;
                    if (!_contexts.TryGetValue(request.Type, out context))
                    {
                        var dialogObject = _factory.Create(request.Type);
                        context = new Context() {
                            Dialog = dialogObject.GetComponent<IDialog>(),
                            Object = dialogObject,
                        };
                        _contexts.Add(request.Type, context);
                    }
                    return new Request() {
                        Dialog = context.Dialog,
                        Param = request.Param,
                    };
                })
                .Where(x => x.Dialog != null)
                .SelectMany(x => {
                    if (_stack.Count > 0)
                        return _stack.Peek().Dialog.OnCloseAsObservable().First().Select(_ => x);
                    else
                        return Observable.Return(x);
                })
                .Do(x => {
                    _stack.Push(x);
                    _isOpen.Value = true;
                })
                .SelectMany(x => x.Dialog.OnOpenAsObservable().First().Select(_ => x))
                .SelectMany(x => x.Dialog.OnStartAsObservable(x.Param).First())
                .Subscribe()
                .AddTo(_disposable);

            var close = _onClose
                .Where(_ => _stack.Count > 0)
                .SelectMany(x => _stack.Pop().Dialog.OnCloseAsObservable()
                        .First()
                        .Select(_ => x))
                .Publish()
                .RefCount();

            close
                .Where(_ => _stack.Count <= 0)
                .Subscribe(x => {
                    _isOpen.Value = false;
                    _intent.Close(x);
                })
                .AddTo(_disposable);

            close
                .Where(_ => _stack.Count > 0)
                .Select(_ => _stack.Peek())
                .SelectMany(x => x.Dialog.OnResumeAsObservable(x.Param).First())
                .Subscribe()
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }

        #region IDialogModel implementation
        IObservable<GameObject> IDialogModel.OnAddAsObservable()
        {
            return _contexts
                .ObserveAdd()
                .Select(x => x.Value.Object);
        }

        IObservable<int> IDialogModel.OnOpenAsObservable()
        {
            return _isOpen
                .Where(x => x)
                .Select(_ => _stack.Count);
        }

        IObservable<int> IDialogModel.OnCloseAsObservable()
        {
            return _isOpen
                .Where(x => !x)
                .Select(_ => _stack.Count);
        }
        #endregion
    }
}
