using System;
using UniRx;

namespace SocialGame.Toast
{
    public interface IToastIntent
    {
        IObservable<RequestToast> OnOpenAsObservable();
    }
}
