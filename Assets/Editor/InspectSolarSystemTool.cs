using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;

public class InspectSolarSystemTool
{
    [MenuItem("Tools/Inspect Solar System Children")]
    public static void Inspect()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("=== SOLAR SYSTEM CHILDREN INSPECTION ===");

        GameObject solarSystem = GameObject.Find("ImageTarget_SolarSystem");
        if (solarSystem == null)
        {
            Debug.LogError("ImageTarget_SolarSystem not found!");
            return;
        }

        InspectRecursive(solarSystem.transform, sb, 0);

        string report = sb.ToString();
        Debug.Log(report);
        File.WriteAllText("SolarSystemInspection.txt", report);
        Debug.Log("Report saved to SolarSystemInspection.txt");
    }

    private static void InspectRecursive(Transform t, StringBuilder sb, int depth)
    {
        string indent = new string(' ', depth * 2);
        sb.AppendLine($"{indent}[{t.name}]");
        sb.AppendLine($"{indent}  LocalPos: {t.localPosition}");
        sb.AppendLine($"{indent}  LocalScale: {t.localScale}");
        sb.AppendLine($"{indent}  WorldPos: {t.position}");
        sb.AppendLine($"{indent}  LossyScale: {t.lossyScale}");
        
        MeshRenderer mr = t.GetComponent<MeshRenderer>();
        if (mr != null)
        {
            sb.AppendLine($"{indent}  MeshRenderer: enabled={mr.enabled}, bounds={mr.bounds.size}");
        }

        MeshFilter mf = t.GetComponent<MeshFilter>();
        if (mf != null && mf.sharedMesh != null)
        {
            sb.AppendLine($"{indent}  Mesh: {mf.sharedMesh.name}, bounds={mf.sharedMesh.bounds.size}");
        }

        for (int i = 0; i < t.childCount; i++)
        {
            InspectRecursive(t.GetChild(i), sb, depth + 1);
        }
    }
}
