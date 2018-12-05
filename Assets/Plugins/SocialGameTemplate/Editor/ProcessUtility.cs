using System;
using System.IO;
using System.Diagnostics;
using UnityEngine;
using UnityEditor;
using Debug = UnityEngine.Debug;

namespace SocialGame.Editor
{
    internal static class ProcessUtility
    {
        public static string DoBashCommand(string cmd)
        {
            var p = new Process();
    
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    p.StartInfo.FileName = Environment.GetEnvironmentVariable("ComSpec");
                    p.StartInfo.Arguments = $"/c \"{cmd}\"";
                    break;
                case RuntimePlatform.OSXEditor:
                    p.StartInfo.FileName = "/bin/bash";
                    p.StartInfo.Arguments = $"-c \"/Library/Frameworks/Mono.framework/Commands/mono {cmd}\"";
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

        public static string DoExeCommand(string cmd, string argsFormat, params string[] args)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    for (var i = 0; i < args.Length; i++)
                        args[i] = $"\"{args[i].Replace('\\', '/')}\"";
                    return DoBashCommand($"\"{cmd.Replace('\\', '/')}\" {string.Format(argsFormat, args)}");
                case RuntimePlatform.OSXEditor:
                    return DoBashCommand($"-c \"/Library/Frameworks/Mono.framework/Commands/mono {cmd} {string.Format(argsFormat, args)}\"");
                default:
                    Debug.LogError("This platform is not supported");
                    return null;
            }
        }

        public static void DoBuildDllCommand(string dst, string[] src)
        {
            var p = new Process();


            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    p.StartInfo.FileName = Environment.GetEnvironmentVariable("ComSpec");
                    p.StartInfo.Arguments = $"/c \"\"{EditorApplication.applicationContentsPath.Replace('\\', '/')}/MonoBleedingEdge/bin/mcs\" /target:library /out:\"{dst.Replace('\\', '/')}\" \"{ string.Join("\" \"", src).Replace('\\', '/')}\"\"";
                    break;
                case RuntimePlatform.OSXEditor:
                    p.StartInfo.FileName = "/bin/bash";
                    p.StartInfo.Arguments = $"-c \"/Library/Frameworks/Mono.framework/Commands/mcs /target:library /out:{dst} {string.Join(" ", src)}\"";
                    break;
                default:
                    Debug.LogError("This platform is not supported");
                    break;
            }
            
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.Start();

            p.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                    Debug.Log("output>>" + e.Data);
            };
            p.BeginOutputReadLine();

            p.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                    Debug.Log("error>>" + e.Data);
            };
            p.BeginErrorReadLine();
            
            p.WaitForExit();
            p.Close();
        }
    }
}
