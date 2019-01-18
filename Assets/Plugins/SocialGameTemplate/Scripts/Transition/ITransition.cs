using UniRx.Async;

namespace SocialGame.Transition
{
    public interface ITransition
    {
        UniTask OnTransIn(float defaultDuration);

        UniTask OnTransOut(float defaultDuration);
    }
}
