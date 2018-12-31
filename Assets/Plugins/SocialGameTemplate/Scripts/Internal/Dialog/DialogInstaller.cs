using SocialGame.Internal.Dialog.Builtin;
using UnityEngine;
using Zenject;

namespace SocialGame.Internal.Dialog
{
    internal sealed class DialogInstaller : MonoInstaller<DialogInstaller>
    {
        [SerializeField] private SampleDialog _sample = null;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<DialogModel>().AsSingle();
            Container.BindInterfacesTo<DialogFactory>().AsSingle();
            Container.BindInstance(_sample).AsSingle();
        }
    }
}
