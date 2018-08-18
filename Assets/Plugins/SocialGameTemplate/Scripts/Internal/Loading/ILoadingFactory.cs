using System;
using SocialGame.Loading;
using UnityEngine;

namespace SocialGame.Internal.Loading
{
    internal interface ILoadingFactory
    {
        GameObject Create(LoadingType type);
    }
}
