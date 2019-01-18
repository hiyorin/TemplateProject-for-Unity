using UniRx.Async;

namespace SocialGame.Toast
{
    public interface IToast
    {
        UniTask OnOpen(object param, float defaultDuration);

        UniTask OnClose(float defaultDuration);
    }
}
