using UnityEditor;

public class ExportPackage
{
    private readonly static string[] Paths = {
        "Assets/Plugins/SocialGameTemplete",
    };

    [MenuItem("Assets/Export SocialGameTemplete")]
    private static void Export()
    {
        AssetDatabase.ExportPackage(Paths, "SocialGameTemplete.unitypackage", ExportPackageOptions.Recurse);
    }
}
