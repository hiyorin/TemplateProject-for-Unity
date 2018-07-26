using UnityEngine;
using Zenject;

namespace SocialGame.Dialog
{
    public sealed class DialogFactory
    {
        [Inject] private DiContainer _container = null;

        [Inject] private DialogSettings _settings = null;

        [Inject] private SampleDialog _samplePrefab = null;

        public GameObject Create(DialogType type)
        {
            if (type == DialogType.Sample)
                return _container.InstantiatePrefab(_samplePrefab);
            else
                return _container.InstantiatePrefab(_settings.Prefabs[(int)type]);
        }
    }
}
