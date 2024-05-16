using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
 
public static class ExportPackage {
 

    [MenuItem("Export/Export with tags and layers, Input settings")]
    public static void export()
    {
        string[] projectContent = new string[] {"Assets", 
        "ProjectSettings/TagManager.asset",
        "ProjectSettings/InputManager.asset",
        "ProjectSettings/ProjectSettings.asset",
        "Packages/com.unity.cinemachine", 
        "Packages/com.unity.inputsystem"};
         
        // Export package
        AssetDatabase.ExportPackage(projectContent, "El_Pollo_Loco.unitypackage", ExportPackageOptions.Interactive | ExportPackageOptions.Recurse | ExportPackageOptions.IncludeDependencies);
        Debug.Log("Project Exported with " + UnityEngine.SceneManagement.SceneManager.loadedSceneCount + " open scenes.");
    }

 
}