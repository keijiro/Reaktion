using UnityEngine;
using UnityEditor;

public class PackageTool
{
    [MenuItem("Package/Update Package")]
    static void UpdatePackage()
    {
        string[] pathNames = { "Assets/Reaktion", "Assets/MidiJack", "Assets/OscJack" };
        AssetDatabase.ExportPackage(pathNames, "Reaktion.unitypackage", ExportPackageOptions.Recurse);
    }
}
