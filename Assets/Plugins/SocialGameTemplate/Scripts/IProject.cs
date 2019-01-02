using System;
using UniRx;

namespace SocialGame
{
    public interface IProject
    {
        IObservable<Unit> OnInitializedAsObservable();
    }
}