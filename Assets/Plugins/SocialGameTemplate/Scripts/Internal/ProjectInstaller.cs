using MessagePack.Resolvers;
using SocialGame.Scene;
using SocialGame.Transition;
using SocialGame.Sound;
using SocialGame.DebugMode;
using SocialGame.Internal.Data;
using SocialGame.Internal.Data.DataStore;
using SocialGame.Internal.Scene;
using SocialGame.Internal.Toast;
using SocialGame.Internal.Dialog;
using SocialGame.Internal.Loading;
using SocialGame.Internal.Network;
using SocialGame.Internal.TapEffect;
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

        [SerializeField] private ResolutionLocalStorage _resolutionStorage;

        private void Awake()
        {
	        CompositeResolver.RegisterAndSetAsDefault(
		        GeneratedResolver.Instance,
		        MessagePack.Unity.UnityResolver.Instance,
		        BuiltinResolver.Instance,
		        AttributeFormatterResolver.Instance,
		        PrimitiveObjectResolver.Instance
	        );
        }
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<ProjectModel>().AsSingle();
            Container.BindInstance<ISceneManager>(_sceneManager).AsSingle();
            Container.BindInterfacesAndSelfTo<TransitionController>().AsSingle();
            Container.BindInterfacesTo<DialogController>().AsSingle();
            Container.BindInterfacesTo<ToastController>().AsSingle();
            Container.BindInterfacesTo<TapEffectController>().AsSingle();
            Container.BindInterfacesTo<LoadingController>().AsSingle();
            Container.BindInterfacesAndSelfTo<SoundController>().AsSingle();
            Container.BindInterfacesAndSelfTo<SoundVolumeController>().AsSingle();
            Container.BindInterfacesAndSelfTo<DebugController>().AsSingle();
            Container.BindInstance(_uiCamera).AsSingle();
            Container.BindInstance(_eventSystem).AsSingle();
            
            // data
            Container.BindInstance(_resolutionStorage).AsSingle();
            Container.BindInterfacesTo<ResolutionLocalDataStore>().AsSingle();
            Container.BindInterfacesTo<ResolutionController>().AsSingle();

            var resolutionSettings = Resources.Load<ResolutionSettings>(ResolutionSettings.FileName);
            Container.BindInstance(resolutionSettings).AsSingle();
            
            var projectSettings = Resources.Load<ProjectSettings>(ProjectSettings.FileName);
            Container.BindInstance(projectSettings.Application).AsSingle();
            Container.BindInstance(projectSettings.Debug).AsSingle();

            // network
            var networkSettings = Resources.Load<NetworkSettings>(NetworkSettings.FileName);
            Container.BindInstance(networkSettings.General);
            Container.BindInstance(networkSettings.Http);
            Container.BindInterfacesTo<HttpConnection>().AsSingle();

            // Debug mode
            Container.BindInterfacesTo<DebugMode.FPSModel>().AsSingle();
            Container.BindInterfacesTo<DebugMode.MemoryModel>().AsSingle();
            Container.BindInterfacesTo<DebugMode.ExtensionModel>().AsSingle();
        }
    }
}
