using System;
using SocialGame.TapEffect;
using UnityEngine;

namespace SocialGame.Internal.TapEffect
{
    internal interface ITapEffectFactory
    {
        GameObject Create(TapEffectType type);
    }
}
