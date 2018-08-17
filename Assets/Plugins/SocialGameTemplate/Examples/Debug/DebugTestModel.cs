using System;
using SocialGame.DebugMode;
using Zenject;
using UniRx;

namespace Sandbox.DebugMode
{
    public sealed class DebugTestModel : IInitializable, IDisposable
    {
        [Inject] private DebugController _controller = null;

        private long _counter;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        void IInitializable.Initialize()
        {
            _controller.AddExtension(this);

            Observable
                .EveryUpdate()
                .Subscribe(x => _counter = x)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _controller.RemoveExtension(this);
            _disposable.Dispose();
        }

        public override string ToString()
        {
            return "DebugTestModel " + _counter;
        }
    }
}
