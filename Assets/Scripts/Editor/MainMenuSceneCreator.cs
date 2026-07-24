using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public class MainMenuSceneCreator : EditorWindow
{
    static MainMenuSceneCreator()
    {
        EditorApplication.delayCall += () =>
        {
            var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>("Assets/Scenes/MainMenuScene.unity");
            if (sceneAsset != null)
            {
                EditorSceneManager.playModeStartScene = sceneAsset;
            }
        };
    }
    [MenuItem("Tools/Generate Main Menu Scene")]
    public static void GenerateMainMenuScene()
    {
        Debug.Log("MainMenuSceneCreator: Starting Main Menu Scene generation...");

        // 1. Create a fresh scene
        var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        // Remove the default Main Camera and Directional Light since we want clean setup
        var mainCam = GameObject.Find("Main Camera");
        if (mainCam != null) DestroyImmediate(mainCam);
        var dirLight = GameObject.Find("Directional Light");
        if (dirLight != null) DestroyImmediate(dirLight);

        // Create clean 2D Camera
        GameObject camGo = new GameObject("MainCamera", typeof(Camera));
        var cam = camGo.GetComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = 5f;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = Color.black;
        camGo.transform.position = new Vector3(0f, 0f, -10f);

        // 2. Instantiate Main Menu Manager
        GameObject managerGo = new GameObject("MainMenuManager", typeof(MainMenuManager));
        
        // 3. Ensure Assets/Scenes directory exists
        string sceneDir = "Assets/Scenes";
        if (!Directory.Exists(sceneDir))
        {
            Directory.CreateDirectory(sceneDir);
        }

        string scenePath = Path.Combine(sceneDir, "MainMenuScene.unity");

        // 4. Save the scene
        bool saveSuccess = EditorSceneManager.SaveScene(scene, scenePath);
        if (!saveSuccess)
        {
            Debug.LogError("MainMenuSceneCreator: Failed to save the Main Menu scene!");
            return;
        }

        Debug.Log($"MainMenuSceneCreator: Saved scene successfully to {scenePath}");

        // 5. Add to Build Settings
        RegisterScenesInBuildSettings(scenePath);

        Debug.Log("MainMenuSceneCreator: Main Menu Scene generation complete!");
    }

    private static void RegisterScenesInBuildSettings(string mainMenuPath)
    {
        string basicScenePath = "Assets/Scenes/BasicScene.unity";

        var buildScenes = new List<EditorBuildSettingsScene>();

        // Ensure MainMenuScene is index 0
        buildScenes.Add(new EditorBuildSettingsScene(mainMenuPath, true));

        // Ensure BasicScene is index 1
        if (File.Exists(basicScenePath))
        {
            buildScenes.Add(new EditorBuildSettingsScene(basicScenePath, true));
            Debug.Log($"MainMenuSceneCreator: Registered BasicScene at index 1.");
        }
        else
        {
            Debug.LogWarning($"MainMenuSceneCreator: BasicScene not found at {basicScenePath}!");
        }

        EditorBuildSettings.scenes = buildScenes.ToArray();
        Debug.Log($"MainMenuSceneCreator: Updated EditorBuildSettings with {buildScenes.Count} scenes.");
    }
}
