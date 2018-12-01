using System;
using System.IO;
using System.Xml.Linq;
using System.Diagnostics;
using UnityEngine;
using UnityEditor;
using UnityExtensions;
using Debug = UnityEngine.Debug;

namespace SocialGame.Internal
{
    internal static class MessagePackCodeGenerator
    {
        [MenuItem("Tools/MessagePack/Code Generator")]
        private static void CodeGenerate()
        {
            string rootPath = Path.GetFullPath(Application.dataPath + "/../");
            string srcPath = rootPath + "Assembly-CSharp.csproj";
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
            string exe = Application.dataPath + "/" + ProjectModel.RootPath + "/Editor/tools/mpc/mpc.exe";
            string input = dstPath;
            string output = Application.dataPath + "/" + ProjectModel.RootPath + "/Scripts/Internal/MessagePackGenerated.cs";
            string cmd = $"{exe} -i {input} -o {output}";
            var log = DoBashCommand(cmd);
            
            // delete temp csproj file
            File.Delete(dstPath);

            Debug.LogFormat("[Complete] Generated MessagePack Code\n\n{0}", log);
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
        
        private static string DoBashCommand(string cmd)
        {
            var p = new Process();

            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    p.StartInfo.FileName = Environment.GetEnvironmentVariable("ComSpec");
                    p.StartInfo.Arguments = "/c " + cmd;
                    break;
                case RuntimePlatform.OSXEditor:
                    p.StartInfo.FileName = "/bin/bash";
                    p.StartInfo.Arguments = "-c \"/Library/Frameworks/Mono.framework/Commands/mono " + cmd + "\"";
                    break;
                default:
                    Debug.LogError("This platform is not supported");
                    break;
            }
            
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();

            var output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            p.Close();
            
            return output;
        }
    }
}
