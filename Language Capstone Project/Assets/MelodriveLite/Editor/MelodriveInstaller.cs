#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class MelodriveInstallPopup : EditorWindow
{
    void OnGUI()
    {
        EditorGUILayout.LabelField("Melodrive");
        GUILayout.Space(10);
        EditorGUILayout.LabelField("First time installation is required. Would you like to run this now?", EditorStyles.wordWrappedLabel);
        if (GUILayout.Button("Yes!"))
        {
            MelodriveInstaller.Install();
            Close();
        }
        if (GUILayout.Button("Not now."))
            Close();

    }
}

[InitializeOnLoad]
public class MelodriveInstaller : MonoBehaviour {

    static string packageFolder;
    
	static MelodriveInstaller()
    {
        // wait for the first editor update to ensure the scene is fully loaded...
        EditorApplication.update += RunOnce;
    }

    static void RunOnce()
    {
        // remove the update listener
        EditorApplication.update -= RunOnce;

        // Detect installation requirements by checking if the MelodriveInstruments are in the wrong place...
        packageFolder = Path.Combine(Application.dataPath, Path.Combine("MelodriveLite", "MelodriveInstruments"));

        if (Directory.Exists(packageFolder))
        {
            // Show a popup to begin the installation process.
            MelodriveInstallPopup window = ScriptableObject.CreateInstance<MelodriveInstallPopup>();
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 110);
            window.ShowPopup();
        }
    }

    public static void Install()
    {
        Debug.Log("Installing Melodrive...");

        if (!Directory.Exists(Application.streamingAssetsPath))
            Directory.CreateDirectory(Application.streamingAssetsPath);

        string installFolder = Path.Combine(Application.streamingAssetsPath, "MelodriveInstruments");
        if (Directory.Exists(installFolder))
            FileUtil.DeleteFileOrDirectory(installFolder);

        FileUtil.MoveFileOrDirectory(packageFolder, installFolder);
        AssetDatabase.Refresh();

        Debug.Log("Melodrive installed");
    }
}

#endif