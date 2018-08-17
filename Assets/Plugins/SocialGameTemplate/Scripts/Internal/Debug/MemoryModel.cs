using System;
using MemoryInfo;
using Zenject;
using UniRx;
using Info = MemoryInfo.MemoryInfo;

namespace SocialGame.Internal.DebugMode
{
    internal interface IMemoryModel
    {
        IObservable<Info> OnUpdateMomoryInfoAsObservable();
    }

    internal sealed class MemoryModel : IInitializable, IDisposable, IMemoryModel
    {
        [Inject] private DebugSettings _settings = null;

        private readonly MemoryInfoPlugin _plugin = new MemoryInfoPlugin();

        private readonly Subject<Info> _onInfo = new Subject<Info>();

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        void IInitializable.Initialize()
        {
            if (!_settings.Memory)
                return;

            Observable
                .Interval(TimeSpan.FromSeconds(_settings.MemoryUpdateInterval))
                .Subscribe(_ => _onInfo.OnNext(_plugin.GetMemoryInfo()))
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }

        #region IMemoryModel implementation
        IObservable<Info> IMemoryModel.OnUpdateMomoryInfoAsObservable()
        {
            return _onInfo;
        }
        #endregion
    }
}
