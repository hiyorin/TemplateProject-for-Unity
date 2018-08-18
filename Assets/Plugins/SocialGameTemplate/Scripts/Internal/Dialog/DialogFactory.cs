using SocialGame.Dialog;
using UnityEngine;
using Zenject;

namespace SocialGame.Internal.Dialog
{
    internal sealed class DialogFactory : IDialogFactory
    {
        [Inject] private DiContainer _container = null;

        [Inject] private DialogSettings _settings = null;

        [Inject] private SampleDialog _samplePrefab = null;

        #region IDialogFactory implementation
        GameObject IDialogFactory.Create(DialogType type)
        {
            if (type == DialogType.Sample)
                return _container.InstantiatePrefab(_samplePrefab);
            else
                return _container.InstantiatePrefab(_settings.Prefabs[(int)type]);
        }
        #endregion
    }
}
