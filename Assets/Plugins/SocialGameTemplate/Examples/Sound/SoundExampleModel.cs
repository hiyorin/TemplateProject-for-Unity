using System;
using SocialGame.Internal.Sound;
using SocialGame.Sound;
using Zenject;
using UniRx;

namespace SocialGame.Examples.Sound
{
    internal interface ISoundExampleModel
    {
        IObservable<string[]> OnAddBgmAsObservable();
    }
    
    internal sealed class SoundExampleModel : IInitializable, IDisposable, ISoundExampleModel
    {
        [Inject] private ISoundExampleIntent _intent = null;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        void IInitializable.Initialize()
        {
            _intent.OnSelectBgmAsObservable()
                .Subscribe()
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }

        IObservable<string[]> ISoundExampleModel.OnAddBgmAsObservable()
        {
            return Observable.Return(Enum.GetNames(typeof(BGM)));
        }
    }
}