using System;
using UniRx;

namespace SocialGame.Toast
{
    public interface IToast
    {
        IObservable<Unit> OnOpenAsObservable(object param);

        IObservable<Unit> OnCloseAsObservable();
    }
}
