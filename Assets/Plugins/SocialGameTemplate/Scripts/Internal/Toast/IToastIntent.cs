using System;
using SocialGame.Toast;
using UniRx;

namespace SocialGame.Internal.Toast
{
    internal interface IToastIntent
    {
        IObservable<RequestToast> OnOpenAsObservable();
    }
}
