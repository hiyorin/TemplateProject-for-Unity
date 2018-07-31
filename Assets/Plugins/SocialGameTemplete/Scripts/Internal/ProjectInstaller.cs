using SocialGame.Scene;
using UnityEngine;
using Zenject;

namespace SocialGame.Internal
{
    public sealed class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private SceneManager _sceneManager = null;
        [SerializeField] private Camera _uiCamera = null;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ProjectModel>().AsSingle();
            Container.BindInstance<ISceneManager>(_sceneManager).AsSingle();
            Container.BindInterfacesAndSelfTo<TransitionController>().AsSingle();
            Container.BindInterfacesAndSelfTo<DialogController>().AsSingle();
            Container.BindInterfacesAndSelfTo<ToastController>().AsSingle();
            Container.BindInterfacesAndSelfTo<TapEffectController>().AsSingle();
            Container.BindInterfacesAndSelfTo<LoadingController>().AsSingle();
            Container.BindInstance<Camera>(_uiCamera).AsSingle();
        }
    }
}
