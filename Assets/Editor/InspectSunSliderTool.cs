using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;

public class InspectSunSliderTool
{
    [MenuItem("Tools/10. Inspect Sun Slider")]
    public static void Inspect()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("=== SUN MARKER FULL INSPECTION ===");

        GameObject sunTarget = GameObject.Find("ImageTarget_Sun");
        if (sunTarget == null)
        {
            sb.AppendLine("ERROR: ImageTarget_Sun not found in the scene.");
        }
        else
        {
            InspectRecursive(sunTarget.transform, sb, 0);
        }

        sb.AppendLine("\n=== UI SLIDER INSPECTION ===");
        GameObject uiManagerObj = GameObject.Find("ARUIManager") ?? GameObject.Find("Canvas");
        if (uiManagerObj != null)
        {
            ARUIManager uiManager = Object.FindAnyObjectByType<ARUIManager>();
            if (uiManager != null)
            {
                if (uiManager.scaleRotationSlider != null)
                {
                    sb.AppendLine($"Slider Found: {uiManager.scaleRotationSlider.name}");
                    sb.AppendLine($"Slider Active: {uiManager.scaleRotationSlider.gameObject.activeInHierarchy}");
                    sb.AppendLine($"Slider Value: {uiManager.scaleRotationSlider.value}");
                }
                else
                {
                    sb.AppendLine("Slider in ARUIManager is NULL!");
                }
            }
            else
            {
                sb.AppendLine("ARUIManager not found.");
            }
        }

        string report = sb.ToString();
        Debug.Log(report);
        File.WriteAllText("SunInspectionReport.txt", report);
        Debug.Log("Saved to SunInspectionReport.txt");
    }

    private static void InspectRecursive(Transform t, StringBuilder sb, int depth)
    {
        string indent = new string(' ', depth * 2);
        sb.AppendLine($"{indent}▶ [{t.name}]");
        sb.AppendLine($"{indent}  - LocalScale: {t.localScale}");

        Component[] components = t.GetComponents<Component>();
        foreach (Component c in components)
        {
            if (c == null || c is Transform) continue;

            if (c is Animator anim)
            {
                string ctrl = anim.runtimeAnimatorController != null ? anim.runtimeAnimatorController.name : "None";
                sb.AppendLine($"{indent}  - [Animator] Controller: {ctrl}");
            }
            else if (c is PlanetInteractable pi)
            {
                string targetName = pi.targetToScale != null ? pi.targetToScale.name : "NULL (Will use itself)";
                sb.AppendLine($"{indent}  - [PlanetInteractable] targetToScale: {targetName}");
            }
            else if (c is Collider col)
            {
                sb.AppendLine($"{indent}  - [{col.GetType().Name}] enabled: {col.enabled}");
            }
            else
            {
                sb.AppendLine($"{indent}  - [{c.GetType().Name}]");
            }
        }

        for (int i = 0; i < t.childCount; i++)
        {
            InspectRecursive(t.GetChild(i), sb, depth + 1);
        }
    }
}
