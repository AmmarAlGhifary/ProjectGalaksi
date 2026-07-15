using UnityEngine;
using UnityEditor;

public class FixSinglePlanetsTool
{
    [MenuItem("Tools/12. Ultimate Fix Single Planets")]
    public static void Fix()
    {
        FixMarker("ImageTarget_Earth", "Earth", "Earth is the third planet from the Sun.");
        FixMarker("ImageTarget_Moon", "Moon", "The Moon is Earth's only natural satellite.");
        FixMarker("ImageTarget_Sun", "Sun", "The Sun is the star at the center of the Solar System.");
        
        // Restore the pulsing animation!
        AnimationClip actionClip = AssetDatabase.LoadAssetAtPath<AnimationClip>("Assets/Animations/Planet_Action.anim");
        if (actionClip != null)
        {
            AnimationCurve scaleCurve = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 1.3f), new Keyframe(2f, 1f));
            actionClip.SetCurve("", typeof(Transform), "localScale.x", scaleCurve);
            actionClip.SetCurve("", typeof(Transform), "localScale.y", scaleCurve);
            actionClip.SetCurve("", typeof(Transform), "localScale.z", scaleCurve);
            EditorUtility.SetDirty(actionClip);
        }
        
        AssetDatabase.SaveAssets();
        UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
        Debug.Log("Single planets fixed! Sliders will work and animations are absolutely unified.");
    }

    private static void FixMarker(string targetName, string planetName, string info)
    {
        GameObject target = GameObject.Find(targetName);
        if (target == null) return;

        // 1. Find the actual Mesh object (Earth, Moon, or Sun)
        Transform meshTransform = null;
        MeshRenderer[] renderers = target.GetComponentsInChildren<MeshRenderer>(true);
        foreach (MeshRenderer r in renderers)
        {
            if (r.gameObject.name.Contains(planetName))
            {
                meshTransform = r.transform;
                break;
            }
        }

        if (meshTransform == null) return;

        // Unpack prefab if it's part of one, so we can restructure it safely
        if (PrefabUtility.IsPartOfAnyPrefab(meshTransform.gameObject))
        {
            PrefabUtility.UnpackPrefabInstance(PrefabUtility.GetOutermostPrefabInstanceRoot(meshTransform.gameObject), PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        }

        // 2. Create or find ScaleWrapper
        Transform scaleWrapper = target.transform.Find("ScaleWrapper");
        if (scaleWrapper == null)
        {
            GameObject wrapperObj = new GameObject("ScaleWrapper");
            scaleWrapper = wrapperObj.transform;
            scaleWrapper.SetParent(target.transform);
            scaleWrapper.localPosition = Vector3.zero;
            scaleWrapper.localRotation = Quaternion.identity;
            
            // If the mesh was huge (like 100), we probably want the wrapper to be small so it fits the screen
            scaleWrapper.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }

        // 3. Move the mesh under the wrapper and center it
        meshTransform.SetParent(scaleWrapper);
        meshTransform.localPosition = Vector3.zero;

        // 4. Clean up old components on the mesh
        Animator meshAnim = meshTransform.GetComponent<Animator>();
        if (meshAnim == null) meshAnim = meshTransform.gameObject.AddComponent<Animator>();
        meshAnim.runtimeAnimatorController = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>("Assets/Animations/UniversalPlanetController.controller");

        PlanetInteractable oldPI = meshTransform.GetComponent<PlanetInteractable>();
        if (oldPI != null) Object.DestroyImmediate(oldPI);
        
        Orbit oldOrbit = meshTransform.GetComponent<Orbit>();
        if (oldOrbit != null) Object.DestroyImmediate(oldOrbit);

        // 5. Set up the Wrapper (Collider MUST be added before PlanetInteractable!)
        BoxCollider col = scaleWrapper.GetComponent<BoxCollider>();
        if (col == null) col = scaleWrapper.gameObject.AddComponent<BoxCollider>();
        col.size = new Vector3(20, 20, 20); // Big enough to tap

        PlanetInteractable wrapperPI = scaleWrapper.GetComponent<PlanetInteractable>();
        if (wrapperPI == null) wrapperPI = scaleWrapper.gameObject.AddComponent<PlanetInteractable>();
        
        wrapperPI.planetName = planetName;
        wrapperPI.generalInformation = info;
        wrapperPI.animator = meshAnim; // Link the child's animator!
        wrapperPI.targetToScale = scaleWrapper; // Slider scales the wrapper!

        // 6. Delete any other junk children of the ImageTarget (like old empty containers)
        for (int i = target.transform.childCount - 1; i >= 0; i--)
        {
            Transform child = target.transform.GetChild(i);
            if (child != scaleWrapper && child.GetComponent<DefaultObserverEventHandler>() == null)
            {
                // Be careful not to delete ImageTarget internal elements
                if (!child.name.Contains("ImageTargetPreview"))
                {
                    Object.DestroyImmediate(child.gameObject);
                }
            }
        }
    }
}
