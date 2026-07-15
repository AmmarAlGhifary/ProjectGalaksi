using UnityEngine;
using UnityEditor;

public class CleanUpSolarSystemTool
{
    [MenuItem("Tools/8. Final Cleanup")]
    public static void CleanUp()
    {
        GameObject solarTarget = GameObject.Find("ImageTarget_SolarSystem");
        if (solarTarget == null) return;

        // 1. Remove rogue PlanetInteractable from the root ImageTarget
        PlanetInteractable rootPI = solarTarget.GetComponent<PlanetInteractable>();
        if (rootPI != null) Object.DestroyImmediate(rootPI);

        // 2. Locate the ScaleWrapper
        Transform scaleWrapper = solarTarget.transform.Find("ScaleWrapper");
        if (scaleWrapper == null) return;

        // Ensure ScaleWrapper has exactly one PlanetInteractable
        PlanetInteractable wrapperPI = scaleWrapper.GetComponent<PlanetInteractable>();
        if (wrapperPI == null) wrapperPI = scaleWrapper.gameObject.AddComponent<PlanetInteractable>();
        wrapperPI.planetName = "Solar System";
        wrapperPI.generalInformation = "The Solar System is the gravitationally bound system of the Sun and the objects that orbit it.";

        // Ensure ScaleWrapper has the Animator (and it controls the floating)
        Animator wrapperAnim = scaleWrapper.GetComponent<Animator>();
        if (wrapperAnim == null) wrapperAnim = scaleWrapper.gameObject.AddComponent<Animator>();
        wrapperAnim.runtimeAnimatorController = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>("Assets/Animations/UniversalPlanetController.controller");
        wrapperPI.animator = wrapperAnim;

        // 3. Locate the container
        Transform container = scaleWrapper.Find("Tata Surya Albi try fix");
        if (container != null)
        {
            // Remove Animator from the container itself
            Animator containerAnim = container.GetComponent<Animator>();
            if (containerAnim != null) Object.DestroyImmediate(containerAnim);

            // Remove rogue components from Earth, Moon, and Sun
            Transform[] children = new Transform[] { container.Find("Earth"), container.Find("Moon"), container.Find("Sun") };
            
            foreach (Transform child in children)
            {
                if (child == null) continue;

                // KILL rogue Animators
                Animator childAnim = child.GetComponent<Animator>();
                if (childAnim != null) Object.DestroyImmediate(childAnim);

                // KILL rogue PlanetInteractables
                PlanetInteractable childPI = child.GetComponent<PlanetInteractable>();
                if (childPI != null) Object.DestroyImmediate(childPI);

                // Ensure they still have colliders and Orbits
                if (child.GetComponent<SphereCollider>() == null) child.gameObject.AddComponent<SphereCollider>();
            }

            // Restore the exact local positions so they are separated!
            Transform earth = container.Find("Earth");
            Transform moon = container.Find("Moon");
            Transform sun = container.Find("Sun");

            if (earth != null) earth.localPosition = new Vector3(-2.24f, 0, 0);
            if (moon != null) moon.localPosition = new Vector3(-3.44f, 0.25f, 0);
            if (sun != null) sun.localPosition = Vector3.zero;
        }

        UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
        Debug.Log("Total cleanup complete! Rogue Animators and PlanetInteractables destroyed.");
    }
}
