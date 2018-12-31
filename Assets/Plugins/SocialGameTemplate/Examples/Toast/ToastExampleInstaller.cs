using UnityEngine;
using Zenject;

namespace Sandbox.Toast
{
    internal sealed class ToastExampleInstaller : MonoInstaller<ToastExampleInstaller>
    {
        [SerializeField] private ToastExampleIntent _intent = null;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<ToastExampleModel>().AsSingle();
            Container.BindInstance<IToastExampleIntent>(_intent).AsSingle();
        }
    }
}
