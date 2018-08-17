using System;
using SocialGame.Toast;
using UnityEngine;
using Zenject;

namespace SocialGame.Internal.Toast
{
    internal sealed class ToastFactory
    {
        [Inject] private DiContainer _container = null;

        [Inject] private ToastSettings _settings = null;

        [Inject] private SampleToast _sample = null;

        public GameObject Create(ToastType type)
        {
            if (type == ToastType.Sample)
                return _container.InstantiatePrefab(_sample);
            else
                return _container.InstantiatePrefab(_settings.Prefabs[(int)type]);
        }
    }
}
