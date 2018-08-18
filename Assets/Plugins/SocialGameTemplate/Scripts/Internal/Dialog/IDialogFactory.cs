using System;
using SocialGame.Dialog;
using UnityEngine;

namespace SocialGame.Internal.Dialog
{
    internal interface IDialogFactory
    {
        GameObject Create(DialogType type);   
    }
}
