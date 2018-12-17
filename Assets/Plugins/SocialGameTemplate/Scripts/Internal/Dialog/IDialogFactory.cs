using System;
using SocialGame.Dialog;
using UnityEngine;

namespace SocialGame.Internal.Dialog
{
    internal interface IDialogFactory
    {
        GameObject Spawn(DialogType type);

        void Despawn(DialogType type, GameObject value);
    }
}
