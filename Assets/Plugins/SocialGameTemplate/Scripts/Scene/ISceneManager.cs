using SocialGame.Transition;

namespace SocialGame.Scene
{
    public interface ISceneManager
    {
        /// <summary>
        /// To the next scene.
        /// </summary>
        /// <param name="scene"></param>
        void Next(Scene scene);

        /// <summary>
        /// To the next scene.
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="transData"></param>
        /// <param name="transMode"></param>
        void Next(Scene scene, object transData, TransMode transMode=TransMode.None);

        /// <summary>
        /// To the next scene.
        /// </summary>
        /// <param name="sceneName"></param>
        void Next(string sceneName);

        /// <summary>
        /// To the next scene.
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="transData"></param>
        /// <param name="transMode"></param>
        void Next(string sceneName, object transData, TransMode transMode=TransMode.None);

        /// <summary>
        /// Return to the previous scene.
        /// </summary>
        void Back();

        /// <summary>
        /// Reload the scene.<para/>
        /// Reload ISceneLifecycle. It does not reload the scene file.
        /// </summary>
        void Reload();
    }
}