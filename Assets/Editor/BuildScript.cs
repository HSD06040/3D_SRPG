using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildScript
{
    static string[] SCENES = FindEnabledEditorScenes();

    [MenuItem("Build/Build Windows Standalone")]
    static void PerformBuild()
    {

        //#if Addressable
        //        AddressableAssetSettings.CleanPlayerContent();
        //        AddressableAssetSettings.BuildPlayerContent();
        //#endif

        BuildPipeline.BuildPlayer(FindEnabledEditorScenes(), $"Builds/Windows/Build_{Application.version}.exe", BuildTarget.StandaloneWindows, BuildOptions.None);
    }

    private static string[] FindEnabledEditorScenes()
    {
        List<string> EditorScenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            EditorScenes.Add(scene.path);
        }
        return EditorScenes.ToArray();
    }
}
