using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;

public class DeepInspectionTool
{
    [MenuItem("Tools/7. Deep Inspection")]
    public static void Inspect()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("=== DEEP HIERARCHY INSPECTION ===");

        GameObject solarSystem = GameObject.Find("ImageTarget_SolarSystem");
        if (solarSystem == null)
        {
            Debug.LogError("ImageTarget_SolarSystem not found!");
            return;
        }

        InspectRecursive(solarSystem.transform, sb, 0);

        string report = sb.ToString();
        Debug.Log(report);
        File.WriteAllText("DeepInspectionReport.txt", report);
        Debug.Log("Report saved to DeepInspectionReport.txt");
    }

    private static void InspectRecursive(Transform t, StringBuilder sb, int depth)
    {
        string indent = new string(' ', depth * 2);
        sb.AppendLine($"{indent}▶ [{t.name}]");
        sb.AppendLine($"{indent}  - LocalPos: {t.localPosition}");
        sb.AppendLine($"{indent}  - LocalScale: {t.localScale}");

        // Log Components
        Component[] components = t.GetComponents<Component>();
        foreach (Component c in components)
        {
            if (c == null) continue;
            
            if (c is Transform) continue; // Skip Transform as we logged it

            if (c is Animator anim)
            {
                string controllerName = anim.runtimeAnimatorController != null ? anim.runtimeAnimatorController.name : "None";
                sb.AppendLine($"{indent}  - [Animator] Controller: {controllerName}, isHuman: {anim.isHuman}");
            }
            else if (c is PlanetInteractable pi)
            {
                sb.AppendLine($"{indent}  - [PlanetInteractable] Name: {pi.planetName}");
            }
            else if (c is Orbit orbit)
            {
                string targetName = orbit.target != null ? orbit.target.name : "None";
                sb.AppendLine($"{indent}  - [Orbit] Target: {targetName}, OrbitSpeed: {orbit.orbitSpeed}, RotSpeed: {orbit.rotationSpeed}");
            }
            else if (c is MeshRenderer mr)
            {
                sb.AppendLine($"{indent}  - [MeshRenderer] enabled: {mr.enabled}, worldBounds: {mr.bounds.size}");
            }
            else if (c is BoxCollider bc)
            {
                sb.AppendLine($"{indent}  - [BoxCollider] size: {bc.size}");
            }
            else if (c is SphereCollider sc)
            {
                sb.AppendLine($"{indent}  - [SphereCollider] radius: {sc.radius}");
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
