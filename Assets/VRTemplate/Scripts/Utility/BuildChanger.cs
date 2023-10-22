#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class BuildChanger
{
    [MenuItem("Window/VRTemplate/ChangeSetUp-VR-NotVR-Mode")]
    static void ChangeSetUpVRNotVRMode()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex != 0) { Debug.LogError("it's recommended to do the setup in the initial scene of the build"); return; }
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            GameObject playerPrefab;
            if (player.name.Contains("NotVR Player"))
            {
                playerPrefab = PrefabUtility.InstantiatePrefab(Resources.Load("Player/VR Player")) as GameObject;
                EnableVR();
            }
            else
            {
                playerPrefab = PrefabUtility.InstantiatePrefab(Resources.Load("Player/NotVR Player")) as GameObject;
                DisableVR();
            }

            playerPrefab.transform.position = player.transform.position;
            playerPrefab.transform.rotation = player.transform.rotation;
            playerPrefab.transform.localScale = player.transform.localScale;
            GameObject.DestroyImmediate(player);

            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            EditorSceneManager.SaveOpenScenes();
            Debug.Log("ChangeSetUp-VR-NotVR-Mode made the change success");
        }
        else
        {
            Debug.LogError("ChangeSetUp-VR-NotVR-Mode needs that the scene has the NotVR Player Prefab");
        }
    }

    static void EnableVR()
    {
        //Changes
        UnityEditor.XR.Management.XRGeneralSettingsPerBuildTarget.XRGeneralSettingsForBuildTarget(BuildTargetGroup.Standalone).InitManagerOnStart = true;
        UnityEditor.XR.Management.XRGeneralSettingsPerBuildTarget.XRGeneralSettingsForBuildTarget(BuildTargetGroup.Android).InitManagerOnStart = true;
        //Mode have changes ''fake''
        EditorUtility.SetDirty(UnityEditor.XR.Management.XRGeneralSettingsPerBuildTarget.XRGeneralSettingsForBuildTarget(BuildTargetGroup.Standalone));
        EditorUtility.SetDirty(UnityEditor.XR.Management.XRGeneralSettingsPerBuildTarget.XRGeneralSettingsForBuildTarget(BuildTargetGroup.Android));
        //Save changes
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    static void DisableVR()
    {
        //Changes
        UnityEditor.XR.Management.XRGeneralSettingsPerBuildTarget.XRGeneralSettingsForBuildTarget(BuildTargetGroup.Standalone).InitManagerOnStart = false;
        UnityEditor.XR.Management.XRGeneralSettingsPerBuildTarget.XRGeneralSettingsForBuildTarget(BuildTargetGroup.Android).InitManagerOnStart = false;
        //Mode have changes ''fake''
        EditorUtility.SetDirty(UnityEditor.XR.Management.XRGeneralSettingsPerBuildTarget.XRGeneralSettingsForBuildTarget(BuildTargetGroup.Standalone));
        EditorUtility.SetDirty(UnityEditor.XR.Management.XRGeneralSettingsPerBuildTarget.XRGeneralSettingsForBuildTarget(BuildTargetGroup.Android));
        //Save changes
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
#endif