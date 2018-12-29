using System;
using UniRx;

namespace SocialGame.Sound
{
    public interface ISoundController
    {
        IObservable<Unit> OnInitializedAsObservable();
        
        void PlayBGM(BGM value);

        void PlayBGM(string name);

        void StopBGM();

        void PlaySE(SE value);

        void PlaySE(string name);
        
        void PlayVoice(Voice value);

        void PlayVoice(string name);
        
        void StopVoice();
    }
}