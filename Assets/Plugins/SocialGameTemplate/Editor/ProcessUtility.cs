using System;
using System.Diagnostics;
using UnityEngine;
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
                    p.StartInfo.Arguments = "/c " + cmd;
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

        public static string DoBuildDllCommand(string dst, string src)
        {
            var p = new Process();

            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
//                    p.StartInfo.FileName = Environment.GetEnvironmentVariable("ComSpec");
//                    p.StartInfo.Arguments = "/c " + cmd;
                    break;
                case RuntimePlatform.OSXEditor:
                    p.StartInfo.FileName = "/bin/bash";
                    p.StartInfo.Arguments = $"-c \"/Library/Frameworks/Mono.framework/Commands/mcs /target:library /out:{dst} {src}\"";
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
