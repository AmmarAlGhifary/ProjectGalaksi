using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class AddPlanetInteractableTool : EditorWindow
{
    [MenuItem("Tools/4. Fix Missing Planet Interactables")]
    public static void FixMissingInteractables()
    {
        int addedCount = 0;

        addedCount += CheckAndAdd("ImageTarget_Earth", "Earth");
        addedCount += CheckAndAdd("ImageTarget_Moon", "Moon");
        addedCount += CheckAndAdd("ImageTarget_Sun", "Sun");

        if (addedCount > 0)
        {
            EditorSceneManager.MarkAllScenesDirty();
            Debug.Log($"[FixTool] Berhasil menambahkan {addedCount} komponen PlanetInteractable baru ke dalam scene.");
        }
        else
        {
            Debug.Log("[FixTool] Semua target sudah memiliki PlanetInteractable, tidak ada yang perlu ditambahkan.");
        }
    }

    private static int CheckAndAdd(string targetName, string planetName)
    {
        GameObject targetObj = GameObject.Find(targetName);
        if (targetObj != null)
        {
            // Kita harus menaruh script di child (model 3D), bukan di ImageTarget langsung
            // untuk menghindari masalah skala Vuforia
            if (targetObj.transform.childCount > 0)
            {
                Transform childTransform = targetObj.transform.GetChild(0);
                
                PlanetInteractable interactable = childTransform.GetComponent<PlanetInteractable>();
                if (interactable == null)
                {
                    // Pastikan objek memiliki Collider, karena PlanetInteractable butuh Collider (RequireComponent)
                    Collider col = childTransform.GetComponent<Collider>();
                    if (col == null)
                    {
                        childTransform.gameObject.AddComponent<SphereCollider>();
                    }

                    interactable = childTransform.gameObject.AddComponent<PlanetInteractable>();
                    if (interactable != null)
                    {
                        interactable.planetName = planetName;
                    }
                    
                    // Add Orbit script automatically if missing just in case
                    if (childTransform.GetComponent<Orbit>() == null)
                    {
                        childTransform.gameObject.AddComponent<Orbit>();
                    }

                    EditorUtility.SetDirty(childTransform.gameObject);
                    return 1;
                }
            }
            else
            {
                Debug.LogWarning($"[FixTool] {targetName} tidak memiliki objek anak (3D Model)!");
            }
        }
        else
        {
            Debug.LogWarning($"[FixTool] Tidak bisa menemukan {targetName} di dalam scene.");
        }
        
        return 0;
    }
}
