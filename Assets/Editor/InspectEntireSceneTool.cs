using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;

public class InspectEntireSceneTool
{
    [MenuItem("Tools/11. Inspect Entire Scene")]
    public static void Inspect()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("=== FULL SCENE INSPECTION REPORT ===");

        // Get root objects
        GameObject[] rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        
        foreach (GameObject rootObj in rootObjects)
        {
            InspectRecursive(rootObj.transform, sb, 0);
        }

        string report = sb.ToString();
        File.WriteAllText("FullSceneInspection.txt", report);
        Debug.Log("Saved complete scene hierarchy to FullSceneInspection.txt. Please share this file or its contents.");
    }

    private static void InspectRecursive(Transform t, StringBuilder sb, int depth)
    {
        string indent = new string(' ', depth * 2);
        sb.AppendLine($"{indent}▶ [{t.name}]");
        sb.AppendLine($"{indent}  - LocalScale: {t.localScale}, LocalPos: {t.localPosition}");

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
                sb.AppendLine($"{indent}  - [PlanetInteractable] Planet: {pi.planetName}, targetToScale: {targetName}");
            }
            else if (c is Collider col)
            {
                sb.AppendLine($"{indent}  - [{col.GetType().Name}] enabled: {col.enabled}");
            }
            else if (c is UnityEngine.UI.Button btn)
            {
                sb.AppendLine($"{indent}  - [Button] Interactable: {btn.interactable}");
            }
            else if (c is UnityEngine.UI.Slider slider)
            {
                sb.AppendLine($"{indent}  - [Slider] Value: {slider.value}, Min: {slider.minValue}, Max: {slider.maxValue}");
            }
            else if (c is UnityEngine.UI.HorizontalLayoutGroup hlg)
            {
                sb.AppendLine($"{indent}  - [HorizontalLayoutGroup] Padding: {hlg.padding.left},{hlg.padding.right},{hlg.padding.top},{hlg.padding.bottom}");
            }
            else if (c is UnityEngine.UI.ContentSizeFitter csf)
            {
                sb.AppendLine($"{indent}  - [ContentSizeFitter] H: {csf.horizontalFit}, V: {csf.verticalFit}");
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
