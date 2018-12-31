using UnityEngine;
using Zenject;

namespace Sandbox.Dialog
{
    internal sealed class DialogExampleInstaller : MonoInstaller
    {
        [SerializeField] private DialogExampleIntent _intent = null;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<DialogExampleModel>().AsSingle();
            Container.BindInstance<IDialogExampleIntent>(_intent).AsSingle();
        }
    }
}
