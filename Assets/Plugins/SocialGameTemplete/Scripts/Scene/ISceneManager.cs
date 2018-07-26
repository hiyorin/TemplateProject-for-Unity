using SocialGame.Transition;

namespace SocialGame.Scene
{
    public interface ISceneManager
    {
        void Next(Scene scene);

        void Next(Scene scene, object transData, TransMode transMode=TransMode.None);

        void Next(string sceneName);

        void Next(string sceneName, object transData, TransMode transMode=TransMode.None);

        void Back();
    }
}