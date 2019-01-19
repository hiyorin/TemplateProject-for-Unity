using UnityEngine;
using Zenject;

namespace Sandbox.Network
{
    internal sealed class NetworkExampleView : MonoBehaviour
    {
        [Inject] private INetworkExampleModel _model = null;

        private void Start()
        {
            
        }
    }
}