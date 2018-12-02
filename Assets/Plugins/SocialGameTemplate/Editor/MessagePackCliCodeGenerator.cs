using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityExtensions;
using SocialGame.Internal;

namespace SocialGame.Editor
{
    internal static class MessagePackCliCodeGenerator
    {
        private static string[] Paths =
        {
            "Scripts/Data/Entity",
        };

        [MenuItem("Tools/msgpack-cli/Code Generator")]
        private static void Test()
        {
            var tempDllPath = Path.GetFullPath(Application.dataPath + "/../msgpack-cli-temp.dll");
            MCS(tempDllPath);
            MPU(tempDllPath, Application.dataPath + "/Scripts/");
            File.Delete(tempDllPath);
            AssetDatabase.Refresh();
            Debug.LogFormat("[Complete] Generated msgpack-cli Code");
        }

        private static void MCS(string dllPath)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var path in Paths)
            {
                var sources = Directory.GetFiles(Path.Combine(Application.dataPath, path), "*.cs",
                    SearchOption.AllDirectories);
                sources.ForEach(x => sb.Append(x).Append(" "));
            }
            
            ProcessUtility.DoBuildDllCommand(dllPath, sb.ToString());
        }

        private static void MPU(string src, string dst)
        {
            var mpu = Path.Combine(Application.dataPath, ProjectModel.RootPath, "Editor/tools/mpu/mpu.exe");
            var ns = "MsgPack.Serialization.GeneratedSerializers";
            var command = $"{mpu} -s -a -n {ns} -o {dst} {src}";
            var output = ProcessUtility.DoBashCommand(command);

            if (string.IsNullOrEmpty(output))
                return;
            
            var genFiles = output.Split('\n');

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("// this file was auto-generated.");
            sb.AppendLine($"using {ns};");
            sb.AppendLine("namespace MsgPack.Serialization");
            sb.AppendLine("{");
            sb.AppendLine("    public static class MsgPackSerializer");
            sb.AppendLine("    {");
            sb.AppendLine("        static readonly SerializationContext Context = new SerializationContext");
            sb.AppendLine("        {");
            sb.AppendLine("            EnumSerializationMethod = EnumSerializationMethod.ByUnderlyingValue,");
            sb.AppendLine("            SerializationMethod = SerializationMethod.Array,");
            sb.AppendLine("        };");
            sb.AppendLine("        static MsgPackSerializer()");
            sb.AppendLine("        {");

            foreach (var genFile in genFiles)
            {
                if (string.IsNullOrEmpty(genFile))
                    continue;
                var className = Path.GetFileNameWithoutExtension(genFile);
                sb.AppendLine($"            Context.Serializers.Register(new {className}(Context));");
            }

            sb.AppendLine("        }");
            sb.AppendLine("        public static MessagePackSerializer<T> Get<T>()");
            sb.AppendLine("        {");
            sb.AppendLine("            return Context.GetSerializer<T>();");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            var serializer = Path.Combine(Application.dataPath, "Scripts/MsgPack/Serialization");
            File.WriteAllText($"{serializer}/MsgPackSerializer.cs", sb.ToString());
        }
    }
}