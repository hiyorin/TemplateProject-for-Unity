using System;
using UnityEngine;
using SocialGame.Toast;

namespace SocialGame.Internal.Toast
{
    internal interface IToastFactory
    {
        GameObject Create(ToastType type);
    }
}
