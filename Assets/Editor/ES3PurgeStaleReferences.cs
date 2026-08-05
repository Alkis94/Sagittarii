using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

public static class ES3PurgeStaleReferences
{
    // Every stale reference found so far was a colored "label icon" (Library/unity editor
    // resources: sv_label_*) that got swept into a scene's ES3ReferenceMgr as a dependency at
    // some point, then never removed after the icon itself was cleared - because
    // RefreshDependencies() only adds references, it never prunes ones that are no longer used.
    // Rather than chase each stale fileID/scene one at a time, this scans every scene in the
    // project and runs Optimize() (= the "Remove all references not in this scene" button) on
    // its ES3ReferenceMgr, which strips any reference that isn't currently a real dependency.
    private static string[] GetAllScenePaths()
    {
        return AssetDatabase.FindAssets("t:Scene", new[] { "Assets" })
            .Select(AssetDatabase.GUIDToAssetPath)
            .Distinct()
            .OrderBy(p => p)
            .ToArray();
    }

    [MenuItem("Tools/Easy Save 3/Purge Stale sv_label References (All Scenes)", false, 151)]
    static void PurgeStaleReferences()
    {
        var scenePaths = GetAllScenePaths();

        if (!EditorUtility.DisplayDialog(
            "Purge stale ES3 references",
            $"This will open all {scenePaths.Length} scenes in the project, remove any Easy Save 3 reference that is no longer an actual dependency of that scene, and save the scene.\n\nMake sure your project is committed/backed up before continuing.",
            "Ok", "Cancel"))
            return;

        var loadedScenePaths = new string[SceneManager.sceneCount];
        for (int i = 0; i < SceneManager.sceneCount; i++)
            loadedScenePaths[i] = SceneManager.GetSceneAt(i).path;

        var changedScenes = "";
        var untouchedScenes = "";
        var errorScenes = "";

        for (int i = 0; i < scenePaths.Length; i++)
        {
            var path = scenePaths[i];

            if (EditorUtility.DisplayCancelableProgressBar("Purging stale references", path, (float)i / scenePaths.Length))
                break;

            if (!System.IO.File.Exists(path))
            {
                errorScenes += $"{path} (file not found)\n";
                continue;
            }

            var sceneWasOpen = loadedScenePaths.Contains(path);
            Scene scene;

            try
            {
                scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
            }
            catch (Exception e)
            {
                errorScenes += $"{path} (failed to open: {e.Message})\n";
                continue;
            }

            var mgrBase = ES3ReferenceMgr.GetManagerFromScene(scene, false);

            if (mgrBase is ES3ReferenceMgr mgr)
            {
                try
                {
                    var before = mgr.refId.Count;
                    mgr.Optimize();
                    var after = mgr.refId.Count;

                    if (after != before)
                    {
                        changedScenes += $"{path} (removed {before - after} reference(s))\n";
                        EditorSceneManager.MarkSceneDirty(scene);
                    }
                    else
                    {
                        untouchedScenes += $"{path}\n";
                    }
                }
                catch (Exception e)
                {
                    errorScenes += $"{path} (error: {e.Message})\n";
                }
            }
            else
            {
                untouchedScenes += $"{path} (no ES3ReferenceMgr found)\n";
            }

            if (!sceneWasOpen)
            {
                var updateReferencesOnSave = ES3Settings.defaultSettingsScriptableObject.updateReferencesWhenSceneIsSaved;
                ES3Settings.defaultSettingsScriptableObject.updateReferencesWhenSceneIsSaved = false;

                EditorSceneManager.SaveScene(scene);
                EditorSceneManager.CloseScene(scene, true);

                ES3Settings.defaultSettingsScriptableObject.updateReferencesWhenSceneIsSaved = updateReferencesOnSave;
            }
        }

        EditorUtility.ClearProgressBar();

        var summary = "";
        if (changedScenes.Length > 0)
            summary += $"Removed stale references from:\n{changedScenes}\n";
        if (untouchedScenes.Length > 0)
            summary += $"No change needed:\n{untouchedScenes}\n";
        if (errorScenes.Length > 0)
            summary += $"Errors:\n{errorScenes}\n";

        Debug.Log("ES3 stale reference purge complete.\n\n" + summary);
        EditorUtility.DisplayDialog("Purge complete", summary.Length > 0 ? summary : "Nothing to do.", "Ok");
    }
}
