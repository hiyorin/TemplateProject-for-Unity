
namespace SocialGame.Loading
{
    public interface ILoadingController
    {
        void Show(LoadingType type);

        void Show(string name);

        void Hide();
    }
}
