using System;
using System.Text;
using System.Collections.Generic;
using Zenject;
using UniRx;

namespace SocialGame.Internal.DebugMode
{
    internal interface IExtensionModel
    {
        IObservable<string> OnUpdateExtensionAsObservable();
    }

    internal sealed class ExtensionModel : IInitializable, IDisposable, IExtensionModel
    {
        [Inject] private IExtensionIntent _intent = null;

        [Inject] private DebugSettings _settings = null;

        private readonly List<Object> _extensions = new List<Object>();

        private readonly StringReactiveProperty _display = new StringReactiveProperty();

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        void IInitializable.Initialize()
        {
            if (!_settings.Extension)
                return;

            _intent
                .OnAddObservable()
                .Subscribe(x => _extensions.Add(x))
                .AddTo(_disposable);

            _intent
                .OnRemoveObervable()
                .Subscribe(x => _extensions.Remove(x))
                .AddTo(_disposable);

            Observable
                .Interval(TimeSpan.FromSeconds(_settings.ExtensionUpdateInterval))
                .Subscribe(_ => {
                    var builder = new StringBuilder();
                    _extensions.ForEach(x => builder.AppendLine(x.ToString()));
                    _display.Value = builder.ToString();
                })
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }

        #region IExtensionModel implementation
        IObservable<string> IExtensionModel.OnUpdateExtensionAsObservable()
        {
            return _display;
        }
        #endregion
    }
}
