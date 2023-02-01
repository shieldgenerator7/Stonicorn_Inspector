using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEditor;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
using System.Linq;
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

    [MenuItem("SG7/Editor/Mechanics/Autosize BoxColliider2D to tiled sprite")]
    public static void autosizeBC2DtoTiledSprite()
    {
        List<GameObject> gos = Selection.gameObjects.ToList();
        gos = gos.FindAll(go =>
            go.GetComponent<SpriteRenderer>()?.drawMode == SpriteDrawMode.Tiled
            && go.GetComponent<BoxCollider2D>()
            );
        if (gos.Count == 0)
        {
            Debug.LogWarning("Select 1 or more gameobjects with both a SpriteRenderer in Tiled mode, and a BoxCollider2D");
            return;
        }
        int changedCount = 0;
        foreach (GameObject go in gos)
        {
            autosizeBC2DtoTiledSprite(go);
            changedCount++;
        }
        Debug.Log($"Autosized {changedCount} BoxCollider2Ds to SpriteRenderer tiled size");
    }
    public static void autosizeBC2DtoTiledSprite(GameObject go)
    {
        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
        BoxCollider2D bc2d = go.GetComponent<BoxCollider2D>();
        bc2d.size = sr.size;
        EditorUtility.SetDirty(bc2d);
    }

    [MenuItem("SG7/Editor/Mechanics/Auto-extend platform width")]
    public static void autoextendPlatformWidth()
    {
        List<GameObject> gos = Selection.gameObjects.ToList();
        gos = gos.FindAll(go =>
            go.GetComponent<SpriteRenderer>()?.drawMode == SpriteDrawMode.Tiled
            && go.GetComponent<BoxCollider2D>()
            );
        if (gos.Count == 0)
        {
            Debug.LogWarning("Select 1 or more gameobjects with both a SpriteRenderer in Tiled mode, and a BoxCollider2D");
            return;
        }
        int changedCount = 0;
        foreach (GameObject go in gos)
        {
            //Find left and right points
            BoxCollider2D bc2d = go.GetComponent<BoxCollider2D>();
            bc2d.enabled = false;
            RaycastHit2D rch2dRight = Physics2D.Raycast(go.transform.position, go.transform.right);
            Vector2 rightPos = rch2dRight.point;
            RaycastHit2D rch2dLeft = Physics2D.Raycast(go.transform.position, -go.transform.right);
            Vector2 leftPos = rch2dLeft.point;
            bc2d.enabled = true;
            //Move platform to center
            go.transform.position = (rightPos + leftPos) / 2;
            EditorUtility.SetDirty(go);
            //Set size to distance between left and right points
            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            Vector2 size = sr.size;
            size.x = Vector2.Distance(leftPos, rightPos);
            sr.size = size;
            EditorUtility.SetDirty(sr);
            //Update BoxCollider2D
            autosizeBC2DtoTiledSprite(go);
            //Update counter
            changedCount++;
        }
        Debug.Log($"Auto-extended {changedCount} platform widths");
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

    [MenuItem("SG7/Editor/Load or Unload Level Scenes %&S")]
    public static void loadOrUnloadLevelScenes()
    {
        //Find out if all of the scenes are loaded
        List<Scene> levels = getLevelScenes((s) => !String.IsNullOrEmpty(s.name), true);
        bool allLoaded = allLevelScenesLoaded(levels);
        //If any are unloaded, load them all.
        //Else, unload them all.
        loadAllLevelScenes(levels, !allLoaded);
    }
    public static void loadAllLevelScenes(List<Scene> levels, bool load)
    {
        //Load or unload all the level scenes
        levels.ForEach(scene =>
        {
            if (!load)
            {
                //Unload
                EditorSceneManager.CloseScene(scene, false);
            }
            else
            {
                //Load
                try
                {
                    EditorSceneManager.OpenScene(
                        scene.path,
                        OpenSceneMode.Additive
                        );
                    SetExpanded(scene, false);
                }
                catch (ArgumentException ae)
                {
                    Debug.LogError($"scene load error ({scene.name}): {ae}");
                }
            }
        });
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
        List<Scene> levels = getLevelScenes(
            (s) => !String.IsNullOrEmpty(s.name),
            true
            );
        loadAllLevelScenes(levels, false);
        loadAllLevelScenes(levels, true);
        int waitCount = 0;
        const int WAIT_LIMIT = 1000;
        while (!allLevelScenesLoaded(levels))
        {
            new WaitForSecondsRealtime(0.1f);
            waitCount++;
            if (waitCount >= WAIT_LIMIT)
            {
                Debug.LogError("Waited longer than limit for scenes to load, continuing anyway");
                break;
            }
        }

        //Checklist
        bool keepScenesOpen = false;
        //(new List<Func<bool>>()).ForEach(func => keepScenesOpen = keepScenesOpen || func);

        keepScenesOpen = checkTiledHitBoxes() || keepScenesOpen;

        //Cleanup
        EditorSceneManager.SaveOpenScenes();
        if (!keepScenesOpen)
        {
            loadAllLevelScenes(levels, false);
        }
        //Finish
        Debug.Log("Finished all Pre-Build Tasks");
        return keepScenesOpen;
    }

    static bool allLevelScenesLoaded(List<Scene> levels)
        => levels.All(s => s.isLoaded);

    [MenuItem("SG7/Build/Pre-Build/Check Tiled HitBoxes")]
    public static bool checkTiledHitBoxes()
    {
        int changedCount = 0;
        GameObject.FindObjectsOfType<SpriteRenderer>().ToList()
            .FindAll(sr => sr.drawMode == SpriteDrawMode.Tiled)
            .OrderBy(sr => sr.gameObject.scene.buildIndex)
            .ThenBy(sr => sr.name).ToList()
            .ForEach(sr =>
            {
                bool changedSR = false;
                //
                // Check for reasonable sprite size
                //
                Vector2 oldSRSize = sr.size;
                Vector2 newSRSize = sr.size;
                newSRSize.x = Mathf.Round(newSRSize.x * 100) / 100;
                newSRSize.y = Mathf.Round(newSRSize.y * 100) / 100;
                if (newSRSize != oldSRSize)
                {
                    sr.size = newSRSize;
                    Debug.LogWarning(
                        $"Changed {sr.name} sprite size " +
                        $"from ({oldSRSize.x}, {oldSRSize.y}) " +
                        $"to ({newSRSize.x}, {newSRSize.y}).",
                        sr
                        );
                    changedSR = true;
                }
                //
                // Check collider size
                //
                BoxCollider2D bc2d = sr.GetComponent<BoxCollider2D>();
                if (bc2d)
                {
                    Vector2 oldSize = bc2d.size;
                    Vector2 newSize = bc2d.size;
                    //If tiled vertically,
                    if (sr.size.y > sr.size.x * 2)
                    {
                        //Set collider height to match
                        newSize.y = sr.size.y;
                    }
                    //Else: it's tiled horizontally, so
                    else
                    {
                        //Set collider width to match
                        newSize.x = sr.size.x;
                    }
                    if (newSize != oldSize)
                    {
                        bc2d.size = newSize;
                        Debug.LogWarning(
                            $"Changed {sr.name} collider size " +
                            $"from ({oldSize.x}, {oldSize.y}) " +
                            $"to ({newSize.x}, {newSize.y}).",
                            sr
                            );
                        changedSR = true;
                    }
                }
                if (changedSR)
                {
                    EditorUtility.SetDirty(sr);
                    if (bc2d) { EditorUtility.SetDirty(bc2d); }
                    EditorUtility.SetDirty(sr.gameObject);
                    changedCount++;
                }
            });

        if (changedCount > 0)
        {
            Debug.LogWarning(
                $"Tiled sprites changes: Made {changedCount} changes."
                );
        }
        return changedCount > 0;
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
        if (!System.IO.Directory.Exists(defaultPath))
        {
            System.IO.Directory.CreateDirectory(defaultPath);
        }
        //2017-10-19 copied from https://docs.unity3d.com/Manual/BuildPlayerPipeline.html
        // Get filename.
        string buildName = EditorUtility.SaveFilePanel("Choose Location of Built Game", defaultPath, PlayerSettings.productName, extension);

        // User hit the cancel button.
        if (buildName == "")
            return;

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

        // Copy a file from the project folder to the build folder, alongside the built game.
        string resourcesPath = $"{path}/Assets/Resources";

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
