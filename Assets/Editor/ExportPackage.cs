using UnityEngine;
using UnityEditor;
using System.IO;

public class ExportPackage
{
    private readonly static string[] Paths = {
        "Assets/Plugins/SocialGameTemplate",
    };

    private const string ReadMe = "README.md";
    private const string License = "LICENSE";

    [MenuItem("Assets/Export SocialGameTemplate")]
    private static void Export()
    {
        string readmePath = Path.Combine(Application.dataPath, "Plugins/SocialGameTemplate", ReadMe);
        string licensePath = Path.Combine(Application.dataPath, "Plugins/SocialGameTemplate", License);
        File.Copy(Path.Combine(Application.dataPath, "..", ReadMe), readmePath);
        File.Copy(Path.Combine(Application.dataPath, "..", License), licensePath);
        AssetDatabase.Refresh();

        AssetDatabase.ExportPackage(Paths, "SocialGameTemplate.unitypackage", ExportPackageOptions.Recurse);
        Debug.Log("Export complete!");

        File.Delete(readmePath);
        File.Delete(licensePath);
        File.Delete(readmePath + ".meta");
        File.Delete(licensePath + ".meta");
        AssetDatabase.Refresh();
    }
}
