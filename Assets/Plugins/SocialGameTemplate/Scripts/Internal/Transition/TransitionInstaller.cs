using SocialGame.Transition;
using UnityEngine;
using Zenject;

namespace SocialGame.Internal.Transition
{
    internal class TransitionInstaller : MonoInstaller
    {
        [SerializeField] private BlackFadeTransition _blackFade = null;

        public override void InstallBindings()
        {
            // models
            Container.BindInterfacesAndSelfTo<TransitionModel>().AsSingle();

            // factories
            Container.BindInterfacesAndSelfTo<TransitionFactory>().AsSingle();

            // prefabs
            Container.BindInstance(_blackFade).AsSingle();
        }
    }
}
