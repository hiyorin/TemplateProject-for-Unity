using System;
using System.Collections.Generic;
using System.Linq;
using SocialGame.Dialog;
using UnityEngine;
using UnityExtensions;
using Zenject;
using UniRx;
using UniRx.Async;

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

        [Inject] private DialogSettings _settings = null;
        
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
                .Merge(
                    _intent.OnOpenAsObservable().Where(x => x.Primary || !_isOpen.Value),
                    _onOpen)
                .Select(request => {
                    Context context = null;
                    if (!_contexts.TryGetValue(request.Type, out context))
                    {
                        var dialogObject = _factory.Spawn(request.Type);
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
                .Subscribe(x => Open(x).GetAwaiter())
                .AddTo(_disposable);

            _onClose
                .Subscribe(x => Close(x).GetAwaiter())
                .AddTo(_disposable);
            
            _intent.OnClearAsObservable()
                .Subscribe(_ => CloseAll().GetAwaiter())
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
            _contexts.ForEach(x => _factory.Despawn(x.Key, x.Value.Object));
            _contexts.Clear();
        }

        private async UniTask Open(Request request)
        {
            if (request.Dialog == null)
                return;

            if (_stack.Count > 0)
                await _stack.Peek().Dialog.OnClose(_settings.DefaultDuration);

            _stack.Push(request);
            _isOpen.Value = true;
            
            await request.Dialog.OnStart(request.Param);
            await request.Dialog.OnOpen(_settings.DefaultDuration);
        }
        
        private async UniTask Close(object param)
        {
            if (_stack.Count <= 0)
                return;
            
            await _stack.Pop().Dialog.OnClose(_settings.DefaultDuration);
            
            if (_stack.Count > 0)
            {
                await _stack.Peek().Dialog.OnResume(param);
            }
            else if (_stack.Count <= 0)
            {
                // all close
                _isOpen.Value = false;
                _intent.Close(param);
            }
        }

        private async UniTask CloseAll()
        {
            await UniTask.WhenAll(_stack.Select(x => x.Dialog.OnClose(_settings.DefaultDuration)));
            
            _stack.Clear();
            _isOpen.Value = false;
            _intent.Close(null);
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
