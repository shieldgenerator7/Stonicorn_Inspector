using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEditor;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
using UnityEditor.SceneManagement;
using System;
using System.Reflection;
using System.IO;

public class CustomMenu
{
    //2023-01-31: CustomMenu copied from MageDuel.CustomMenu

    const int FIRST_LEVEL_INDEX = 4;

    //Find Missing Scripts
    //2018-04-13: copied from http://wiki.unity3d.com/index.php?title=FindMissingScripts
    static int go_count = 0, components_count = 0, missing_count = 0;
    [MenuItem("SG7/Editor/Refactor/Find Missing Scripts")]
    private static void FindMissingScripts()
    {
        go_count = 0;
        components_count = 0;
        missing_count = 0;
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene s = SceneManager.GetSceneAt(i);
            if (s.isLoaded)
            {
                foreach (GameObject go in s.GetRootGameObjects())
                {
                    FindInGO(go);
                }
            }
        }
        Debug.Log($"Searched {go_count} GameObjects, {components_count} components, found {missing_count} missing");
    }
    private static void FindInGO(GameObject g)
    {
        go_count++;
        Component[] components = g.GetComponents<Component>();
        for (int i = 0; i < components.Length; i++)
        {
            components_count++;
            if (components[i] == null)
            {
                missing_count++;
                string s = g.name;
                Transform t = g.transform;
                while (t.parent != null)
                {
                    s = $"{t.parent.name}/{s}";
                    t = t.parent;
                }
                Debug.Log($"{s} has an empty script attached in position: {i}", g);
            }
        }
        // Now recurse through each child GO (if there are any):
        foreach (Transform childT in g.transform)
        {
            FindInGO(childT.gameObject);
        }
    }
    
    public static List<Scene> getLevelScenes(Func<Scene, bool> filter = null, bool reportFailures = false)
    {
        List<Scene> scenes = new List<Scene>();
        for (int i = FIRST_LEVEL_INDEX; i < EditorBuildSettings.scenes.Length; i++)
        {
            Scene scene = EditorSceneManager.GetSceneByBuildIndex(i);
            if (filter?.Invoke(scene) ?? true)
            {
                scenes.Add(scene);
            }
            else
            {
                if (reportFailures)
                {
                    Debug.LogError($"Scene {scene.name} at index {i} failed the filter.");
                }
            }
        }
        return scenes;
    }

       //2020-12-09: copied from https://forum.unity.com/threads/how-to-collapse-hierarchy-scene-nodes-via-script.605245/#post-6551890
    private static void SetExpanded(Scene scene, bool expand)
    {
        foreach (var window in Resources.FindObjectsOfTypeAll<SearchableEditorWindow>())
        {
            if (window.GetType().Name != "SceneHierarchyWindow")
                continue;

            var method = window.GetType().GetMethod("SetExpandedRecursive",
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance, null,
                new[] { typeof(int), typeof(bool) }, null);

            if (method == null)
            {
                Debug.LogError(
                    "Could not find method 'UnityEditor.SceneHierarchyWindow.SetExpandedRecursive(int, bool)'.");
                return;
            }

            var field = scene.GetType().GetField("m_Handle",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (field == null)
            {
                Debug.LogError("Could not find field 'int UnityEngine.SceneManagement.Scene.m_Handle'.");
                return;
            }

            var sceneHandle = field.GetValue(scene);
            method.Invoke(window, new[] { sceneHandle, expand });
        }
    }

    public static void ClearLog()
    {
        //2020-12-28: copied from https://stackoverflow.com/a/40578161/2336212
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }

    [MenuItem("SG7/Build/Pre-Build/Perform all Pre-Build Tasks &W")]
    public static bool performAllPreBuildTasks()
    {
        ClearLog();
        Debug.Log("Running all Pre-Build Tasks");
        //Setup
        EditorSceneManager.SaveOpenScenes();

        //Checklist
        bool problemFound = false;
        //problemFound = functionCall() || problemFound;

        //Cleanup
        EditorSceneManager.SaveOpenScenes();
        //Finish
        Debug.Log("Finished all Pre-Build Tasks");
        return problemFound;
    }



    /// <summary>
    /// Recursively gather all files under the given path including all its subfolders.
    /// 2021-07-12: copied from http://answers.unity.com/answers/916074/view.html
    /// </summary>
    static IEnumerable<string> GetFiles(string path)
    {
        Queue<string> queue = new Queue<string>();
        queue.Enqueue(path);
        while (queue.Count > 0)
        {
            path = queue.Dequeue();
            try
            {
                foreach (string subDir in Directory.GetDirectories(path))
                {
                    queue.Enqueue(subDir);
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.Message);
            }
            string[] files = null;
            try
            {
                files = Directory.GetFiles(path);
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.Message);
            }
            if (files != null)
            {
                for (int i = 0; i < files.Length; i++)
                {
                    yield return files[i];
                }
            }
        }
    }

    [MenuItem("SG7/Build/Build Windows %w")]
    public static void buildWindows()
    {
        build(BuildTarget.StandaloneWindows, "exe");
    }
    [MenuItem("SG7/Build/Build Linux")]
    public static void buildLinux()
    {
        Debug.LogError(
            "Building Linux has not been readded yet after Unity removed it in 2019.2"
            );
    }
    [MenuItem("SG7/Build/Build Mac OS X")]
    public static void buildMacOSX()
    {
        build(BuildTarget.StandaloneOSX, "");
    }
    public static void build(BuildTarget buildTarget, string extension)
    {
        string defaultPath = getDefaultBuildPath();
        if (!Directory.Exists(defaultPath))
        {
            Directory.CreateDirectory(defaultPath);
        }
        //2017-10-19 copied from https://docs.unity3d.com/Manual/BuildPlayerPipeline.html
        // Get filename.
        string buildName = EditorUtility.SaveFilePanel("Choose Location of Built Game", defaultPath, PlayerSettings.productName, extension);

        // User hit the cancel button.
        if (buildName == "")
        {
            return;
        }

        string path = buildName.Substring(0, buildName.LastIndexOf("/"));
        Debug.Log($"BUILDNAME: {buildName}");
        Debug.Log($"PATH: {path}");

        string[] levels = new string[EditorBuildSettings.scenes.Length];
        for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            if (EditorBuildSettings.scenes[i].enabled)
            {
                levels[i] = EditorBuildSettings.scenes[i].path;
            }
            else
            {
                break;
            }
        }

        // Build player.
        BuildPipeline.BuildPlayer(levels, buildName, buildTarget, BuildOptions.None);

        // Run the game (Process class from System.Diagnostics).
        Process proc = new Process();
        proc.StartInfo.FileName = buildName;
        proc.Start();
    }

    [MenuItem("SG7/Run/Run Windows %#w")]
    public static void runWindows()
    {//2018-08-10: copied from build()
        string extension = "exe";
        string buildName = getBuildNamePath(extension);
        Debug.Log($"Launching: {buildName}");
        // Run the game (Process class from System.Diagnostics).
        Process proc = new Process();
        proc.StartInfo.FileName = buildName;
        proc.Start();
    }

    [MenuItem("SG7/Run/Open Build Folder #w")]
    public static void openBuildFolder()
    {
        string extension = "exe";
        string buildName = getBuildNamePath(extension);
        //Open the folder where the game is located
        EditorUtility.RevealInFinder(buildName);
    }

    [MenuItem("SG7/Run/Open App Data Folder &f")]
    public static void openAppDataFolder()
    {
        string filePath = $"{Application.persistentDataPath}/";
        if (System.IO.File.Exists(filePath))
        {
            EditorUtility.RevealInFinder(filePath);
        }
        else
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }
    }

    public static string getDefaultBuildPath()
    {
        return $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/Unity/{PlayerSettings.productName}/Builds/{PlayerSettings.productName}_{PlayerSettings.bundleVersion.Replace(".", "_")}";
    }
    public static string getBuildNamePath(string extension, bool checkFolderExists = true)
    {
        string defaultPath = getDefaultBuildPath();
        if (checkFolderExists && !System.IO.Directory.Exists(defaultPath))
        {
            throw new UnityException($"You need to build the {extension} for {PlayerSettings.productName} (Version {PlayerSettings.bundleVersion}) first!");
        }
        string buildName = $"{defaultPath}/{PlayerSettings.productName}.{extension}";
        return buildName;
    }

    [MenuItem("SG7/Session/Begin Session")]
    public static void beginSession()
    {
        Debug.Log("=== Beginning session ===");
        string oldVersion = PlayerSettings.bundleVersion;
        string[] split = oldVersion.Split('.');
        string majorVersion = $"{split[0]}";
        string minorVersion = $"{int.Parse(split[1]) + 1}".PadLeft(split[1].Length, '0');
        string newVersion = $"{majorVersion}.{minorVersion}";
        PlayerSettings.bundleVersion = newVersion;
        //Save and Log
        EditorSceneManager.SaveOpenScenes();
        Debug.LogWarning($"Updated build version number from {oldVersion} to {newVersion}");
    }

    [MenuItem("SG7/Session/Finish Session")]
    public static void finishSession()
    {
        Debug.Log("=== Finishing session ===");
        bool problems = performAllPreBuildTasks();
        if (!problems)
        {
            EditorSceneManager.SaveOpenScenes();
            buildWindows();
            //Open folders
            openBuildFolder();
        }
    }

    [MenuItem("SG7/Upgrade/Force save all assets")]
    public static void forceSaveAllAssets()
    {
        AssetDatabase.ForceReserializeAssets();
    }
}
