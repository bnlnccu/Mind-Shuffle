using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

/// <summary>
/// Automatically loads SampleScene when Unity opens the project
/// and no scene is loaded (shows "Untitled").
/// </summary>
[InitializeOnLoad]
public static class AutoLoadScene
{
    static AutoLoadScene()
    {
        EditorApplication.delayCall += () =>
        {
            Scene active = SceneManager.GetActiveScene();
            if (string.IsNullOrEmpty(active.path))
            {
                string scenePath = "Assets/Scenes/SampleScene.unity";
                if (System.IO.File.Exists(scenePath))
                {
                    EditorSceneManager.OpenScene(scenePath);
                }
            }
        };
    }
}
