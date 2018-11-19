using SocialGame.Scene;
using SocialGame.Transition;
using SocialGame.Dialog;
using SocialGame.Toast;
using SocialGame.Loading;
using SocialGame.TapEffect;
using SocialGame.Sound;
using SocialGame.DebugMode;
using SocialGame.Internal.Network;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Editor")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Play")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace SocialGame.Internal
{
    internal sealed class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private SceneManager _sceneManager;

        [SerializeField] private Camera _uiCamera;

        [SerializeField] private EventSystem _eventSystem;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ProjectModel>().AsSingle();
            Container.BindInstance<ISceneManager>(_sceneManager).AsSingle();
            Container.BindInterfacesAndSelfTo<TransitionController>().AsSingle();
            Container.BindInterfacesAndSelfTo<DialogController>().AsSingle();
            Container.BindInterfacesAndSelfTo<ToastController>().AsSingle();
            Container.BindInterfacesAndSelfTo<TapEffectController>().AsSingle();
            Container.BindInterfacesAndSelfTo<LoadingController>().AsSingle();
            Container.BindInterfacesAndSelfTo<SoundController>().AsSingle();
            Container.BindInterfacesAndSelfTo<SoundVolumeController>().AsSingle();
            Container.BindInterfacesAndSelfTo<DebugController>().AsSingle();
            Container.BindInstance(_uiCamera).AsSingle();
            Container.BindInstance(_eventSystem).AsSingle();
            
            var projectSettings = Resources.Load<ProjectSettings>("ProjectSettings");
            Container.BindInstance(projectSettings.Application).AsSingle();
            Container.BindInstance(projectSettings.Debug).AsSingle();

            // network
            var networkSettings = Resources.Load<NetworkSettings>("NetworkSettings");
            Container.BindInstance(networkSettings.General);
            Container.BindInstance(networkSettings.Http);
            Container.BindInterfacesAndSelfTo<HttpConnection>().AsSingle();

            // Debug mode
            Container.BindInterfacesAndSelfTo<DebugMode.FPSModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<DebugMode.MemoryModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<DebugMode.ExtensionModel>().AsSingle();
        }
    }
}
