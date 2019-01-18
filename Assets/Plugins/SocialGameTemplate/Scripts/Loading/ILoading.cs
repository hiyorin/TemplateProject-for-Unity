using UniRx.Async;

namespace SocialGame.Loading
{
    public interface ILoading
    {
        UniTask OnShow(float defaultDuration);

        UniTask OnHide(float defaultDuration);
    }
}
