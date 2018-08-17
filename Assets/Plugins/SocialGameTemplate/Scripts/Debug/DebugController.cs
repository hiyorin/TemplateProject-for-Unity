using System;
using SocialGame.Internal.DebugMode;
using UniRx;

namespace SocialGame.DebugMode
{
    public sealed class DebugController : IExtensionIntent
    {
        private readonly Subject<Object> _onAdd = new Subject<object>();

        private readonly Subject<Object> _onRemove = new Subject<object>();

        public void AddExtension(Object value)
        {
            _onAdd.OnNext(value);
        }

        public void RemoveExtension(Object value)
        {
            _onRemove.OnNext(value);
        }

        #region IExtensionIntent implementation
        IObservable<Object> IExtensionIntent.OnAddObservable()
        {
            return _onAdd;
        }

        IObservable<Object> IExtensionIntent.OnRemoveObervable()
        {
            return _onRemove;
        }
        #endregion
    }
}
