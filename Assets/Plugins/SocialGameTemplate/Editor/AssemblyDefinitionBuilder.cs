using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace SocialGame.Internal
{
    public sealed class AssemblyDefinitionBuilder
    {
        [Serializable]
        private class AssemblyDefinition
        {
            public string name;
            public List<string> references;
            public List<string> optionalUnityReferences;
            public List<string> includePlatforms;
            public List<string> excludePlatforms;
            public bool allowUnsafeCode;
        }

        private readonly string _filePath;

        private readonly AssemblyDefinition _asmdef = new AssemblyDefinition();

        private const string Extension = ".asmdef";

        public AssemblyDefinitionBuilder(string path)
        {
            _filePath = Path.ChangeExtension(Path.Combine(Application.dataPath, path), Extension);
            _asmdef.name = Path.GetFileName(path);
            _asmdef.references = new List<string>();
            _asmdef.optionalUnityReferences = new List<string>();
            _asmdef.includePlatforms = new List<string>();
            _asmdef.excludePlatforms = new List<string>();
            _asmdef.allowUnsafeCode = false;
        }

        public AssemblyDefinitionBuilder AddReferences(params string[] value)
        {
            _asmdef.references.AddRange(value);
            return this;
        }

        public AssemblyDefinitionBuilder EnableTestAssemblies()
        {
            _asmdef.optionalUnityReferences.Add("TestAssemblies");
            return this;
        }

        public AssemblyDefinitionBuilder AddIncludePlatforms(params string[] values)
        {
            _asmdef.includePlatforms.AddRange(values);
            return this;
        }

        public AssemblyDefinitionBuilder AddExcludePlatforms(params string[] values)
        {
            _asmdef.excludePlatforms.AddRange(values);
            return this;
        }

        public AssemblyDefinitionBuilder EnableAllowUnsafeCode()
        {
            _asmdef.allowUnsafeCode = true;
            return this;
        }

        public AssemblyDefinitionBuilder Write()
        {
            var json = JsonUtility.ToJson(_asmdef, true);
            File.WriteAllText(_filePath, json);
            return this;
        }
    }
}
