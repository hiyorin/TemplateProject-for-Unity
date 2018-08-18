using System;
using SocialGame.Transition;
using UnityEngine;

namespace SocialGame.Internal.Transition
{
    internal interface ITransitionFactory
    {
        GameObject Create(TransMode trans);
    }
}
