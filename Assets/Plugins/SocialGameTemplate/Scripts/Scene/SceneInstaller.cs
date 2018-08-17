using UnityEngine;
using Zenject;

namespace SocialGame.Scene
{
    [RequireComponent(typeof(SceneSettings))]
    public abstract class SceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var settings = GetComponent<SceneSettings>();
            Container.BindInstance(settings).AsSingle();

            OnInstallBindings();
        }

        protected abstract void OnInstallBindings();
    }
}
