using System;
using UniRx;

namespace SocialGame.Sound
{
    public interface ISoundController
    {
        IObservable<Unit> OnInitializedAsObservable();
        
        void PlayBGM(BGM value);

        void StopBGM();

        void PlaySE(SE value);

        void PlayVoice(Voice value);
        
        void StopVoice();
    }
}