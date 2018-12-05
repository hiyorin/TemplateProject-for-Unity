using System.IO;
using System;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEditor;
using UnityExtensions;
using SocialGame.Internal;
using Debug = UnityEngine.Debug;

namespace SocialGame.Editor
{
    internal static class MessagePackCodeGenerator
    {
        [MenuItem("Tools/MessagePack-CSharp/Code Generator")]
        private static void CodeGenerate()
        {
            string rootPath = Path.GetFullPath(Application.dataPath + "/../");
            string srcPath;

            if (Application.platform == RuntimePlatform.OSXEditor)
            {
                srcPath = rootPath + "Assembly-CSharp.csproj";
                string dstPath = rootPath + typeof(MessagePackCodeGenerator).Name + ".csproj";

                XDocument document = XDocument.Load(srcPath);
                string ns = "{" + document.Root.Name.Namespace + "}";

                // Insert self references
                var selfItemGroup = GenerateItemGroup(ns, rootPath, "Assets/Plugins/SocialGameTemplate");
                document.Root.AddFirst(selfItemGroup);

                // Insert MessagePack references
                var mgsItemGroup = GenerateItemGroup(ns, rootPath, "Assets/Plugins/MessagePack/");
                document.Root.AddFirst(mgsItemGroup);

                // To avoid many open files error. Remove unused references.
                document.Root.LastNode.Remove();
                document.Root.LastNode.Remove();
                document.Root.LastNode.Remove();
                document.Root.LastNode.Remove();

                document.Save(dstPath);

                // execute mpc process
                string cmd = Application.dataPath + "/" + ProjectModel.RootPath + "/Editor/tools/mpc/mpc.exe";
                string input = dstPath;
                string output = Application.dataPath + "/" + ProjectModel.RootPath + "/Scripts/Internal/MessagePackGenerated.cs";
                string argsFormat = $"-i {{0}} -o {{1}}";
                string log = ProcessUtility.DoExeCommand(cmd, argsFormat, input, output);

                // delete temp csproj file
                File.Delete(dstPath);

                Debug.LogFormat("[Complete] Generated MessagePack Code\n\n{0}", log);
            }
            else
            {
                srcPath = rootPath + $"{rootPath.Split(new[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault()}.csproj";

                // execute mpc process
                string cmd = Application.dataPath + "/" + ProjectModel.RootPath + "/Editor/tools/mpc/mpc.exe";
                string input = srcPath;
                string output = Application.dataPath + "/" + ProjectModel.RootPath + "/Scripts/Internal/MessagePackGenerated.cs";
                string argsFormat = $"-i {{0}} -o {{1}}";
                string log = ProcessUtility.DoExeCommand(cmd, argsFormat, input, output);

                Debug.LogFormat("[Complete] Generated MessagePack Code\n\n{0}", log);
            }
        }
        
        private static XElement GenerateItemGroup(string ns, string rootPath, string dir)
        {
            var sourcePaths = Directory.GetFiles(rootPath + dir, "*.cs", SearchOption.AllDirectories);
            
            for (var i = 0; i < sourcePaths.Length; i++)
                sourcePaths[i] = sourcePaths[i].Replace(rootPath, "");
            
            XElement itemGroup = new XElement(ns + "ItemGroup");
            
            sourcePaths.ForEach(x =>
            {
                var element = new XElement(
                    ns + "Compile",
                    new XAttribute("Include", x));
                itemGroup.Add(element);
            });
            
            return itemGroup;
        }
    }
}
