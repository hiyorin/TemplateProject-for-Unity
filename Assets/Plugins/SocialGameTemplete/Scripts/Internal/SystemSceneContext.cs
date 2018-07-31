using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SocialGame.Internal
{
    public sealed class SystemSceneContext : SceneContext
    {
        [SerializeField] private string _settingsPath = string.Empty;

        protected override void RunInternal()
        {
            var scriptableObjectInstallers = new List<ScriptableObjectInstaller>(ScriptableObjectInstallers);
            scriptableObjectInstallers.Add(Resources.Load<ScriptableObjectInstaller>(_settingsPath));
            ScriptableObjectInstallers = scriptableObjectInstallers;

            base.RunInternal();
        }
    }
}
