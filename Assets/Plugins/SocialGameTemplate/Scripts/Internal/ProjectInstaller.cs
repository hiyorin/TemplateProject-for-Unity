using SocialGame.Scene;
using SocialGame.DebugMode;
using SocialGame.Internal.Scene;
using SocialGame.Internal.Sound;
using SocialGame.Internal.Toast;
using SocialGame.Internal.Dialog;
using SocialGame.Internal.Loading;
using SocialGame.Internal.Network;
using SocialGame.Internal.TapEffect;
using SocialGame.Internal.Transition;
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
        [SerializeField] private SceneManager _sceneManager = null;

        [SerializeField] private Camera _uiCamera = null;

        [SerializeField] private EventSystem _eventSystem = null;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<ProjectModel>().AsSingle();
            Container.BindInstance<ISceneManager>(_sceneManager).AsSingle();
            Container.BindInterfacesTo<TransitionController>().AsSingle();
            Container.BindInterfacesTo<DialogController>().AsSingle();
            Container.BindInterfacesTo<ToastController>().AsSingle();
            Container.BindInterfacesTo<TapEffectController>().AsSingle();
            Container.BindInterfacesTo<LoadingController>().AsSingle();
            Container.BindInterfacesTo<SoundController>().AsSingle();
            Container.BindInterfacesAndSelfTo<DebugController>().AsSingle();
            Container.BindInstance(_uiCamera).AsSingle();
            Container.BindInstance(_eventSystem).AsSingle();

            var resolutionSettings = Resources.Load<ResolutionSettings>(ResolutionSettings.FileName);
            Container.BindInstance(resolutionSettings).AsSingle();
            
            var projectSettings = Resources.Load<ProjectSettings>(ProjectSettings.FileName);
            Container.BindInstance(projectSettings.Application).AsSingle();
            Container.BindInstance(projectSettings.Debug).AsSingle();

            // installer
            Container.Install<NetworkInstaller>();
            
            // Debug mode
            Container.BindInterfacesTo<DebugMode.FPSModel>().AsSingle();
            Container.BindInterfacesTo<DebugMode.MemoryModel>().AsSingle();
            Container.BindInterfacesTo<DebugMode.ExtensionModel>().AsSingle();
        }
    }
}
