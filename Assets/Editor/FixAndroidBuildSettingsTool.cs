using UnityEngine;
using UnityEditor;

public class FixAndroidBuildSettingsTool
{
    [MenuItem("Tools/13. Fix Android Build Settings")]
    public static void FixSettings()
    {
        // 1. Set Minimum API Level to 29 (Android 10.0) as required by Vuforia
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel29;
        
        // 2. Remove Vulkan from Android Graphics APIs (Vuforia doesn't fully support it)
        PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.Android, false);
        
        UnityEngine.Rendering.GraphicsDeviceType[] apis = PlayerSettings.GetGraphicsAPIs(BuildTarget.Android);
        bool hasGLES3 = false;
        
        // Check if OpenGLES3 is already in the list
        foreach (var api in apis)
        {
            if (api == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
            {
                hasGLES3 = true;
                break;
            }
        }

        // We want to strictly use OpenGLES3 for Vuforia Android
        PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new[] { UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3 });

        Debug.Log("Android Build Settings Fixed! Minimum API is now Level 29, and Vulkan has been removed.");
    }
}
