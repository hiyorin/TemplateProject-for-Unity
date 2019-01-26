using UniRx.Async;

namespace SocialGame.Scene
{
    /// <summary>
    /// Provide the life cycle of the scene.
    /// </summary>
    public interface ISceneLifecycle
    {
        /// <summary>
        ///　Load dynamic assets.
        /// </summary>
        /// <param name="transData"></param>
        /// <returns></returns>
        UniTask OnLoad(object transData);
        
        /// <summary>
        /// Interaction at in-transition.
        /// </summary>
        /// <returns></returns>
        UniTask OnTransIn();

        /// <summary>
        /// Completed in-transition.
        /// </summary>
        void OnTransComplete();

        /// <summary>
        /// Interaction at out-transition.
        /// </summary>
        /// <returns></returns>
        UniTask OnTransOut();

        /// <summary>
        ///　Unload dynamic assets.
        /// </summary>
        /// <param name="transData"></param>
        /// <returns></returns>
        UniTask OnUnload();
    }
}
