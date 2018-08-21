using System;
using UniRx;

namespace SocialGame.Internal.Sound
{
    internal interface ISoundVolumeIntent
    {
        IObservable<float> OnMasterVolumeAsObservable();
        IObservable<float> OnBGMVolumeAsObservable();
        IObservable<float> OnSEVolumeAsObservable();
        IObservable<float> OnVoiceVolumeAsObservable();
    }
}
