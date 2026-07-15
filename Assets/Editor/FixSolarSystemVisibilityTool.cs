using UnityEngine;
using UnityEditor;

public class FixSolarSystemVisibilityTool
{
    [MenuItem("Tools/3. Fix Solar System Visibility")]
    public static void FixVisibility()
    {
        // 1. Fix the Solar System marker
        GameObject solarTarget = GameObject.Find("ImageTarget_SolarSystem");
        if (solarTarget != null)
        {
            // Remove PlanetInteractable from the root (which was scaling the marker and breaking Vuforia)
            PlanetInteractable rootPI = solarTarget.GetComponent<PlanetInteractable>();
            if (rootPI != null) Object.DestroyImmediate(rootPI);

            BoxCollider rootCol = solarTarget.GetComponent<BoxCollider>();
            if (rootCol != null) Object.DestroyImmediate(rootCol);

            // Get the container
            Transform container = solarTarget.transform.Find("Tata Surya Albi try fix");
            if (container != null)
            {
                // Add a collider to the container so PlanetInteractable can exist
                BoxCollider col = container.GetComponent<BoxCollider>();
                if (col == null)
                {
                    col = container.gameObject.AddComponent<BoxCollider>();
                    col.size = new Vector3(10, 10, 10);
                }

                PlanetInteractable pi = container.GetComponent<PlanetInteractable>();
                if (pi == null) pi = container.gameObject.AddComponent<PlanetInteractable>();

                pi.planetName = "Solar System";
                pi.generalInformation = "The Solar System is the gravitationally bound system of the Sun and the objects that orbit it.";
                
                Animator anim = container.GetComponent<Animator>();
                if (anim != null) pi.animator = anim;

                // SHRINK the container so Earth and Moon fit inside the camera view!
                container.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            }
        }

        // 2. Remove the scale animation from the Action clip so it doesn't fight the slider
        AnimationClip actionClip = AssetDatabase.LoadAssetAtPath<AnimationClip>("Assets/Animations/Planet_Action.anim");
        if (actionClip != null)
        {
            actionClip.SetCurve("", typeof(Transform), "localScale.x", null);
            actionClip.SetCurve("", typeof(Transform), "localScale.y", null);
            actionClip.SetCurve("", typeof(Transform), "localScale.z", null);
            EditorUtility.SetDirty(actionClip);
            AssetDatabase.SaveAssets();
        }

        UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
        Debug.Log("Solar System visibility fixed! Container scaled down and scripts moved.");
    }
}
