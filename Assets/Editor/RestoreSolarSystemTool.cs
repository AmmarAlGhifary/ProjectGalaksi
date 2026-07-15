using UnityEngine;
using UnityEditor;

public class RestoreSolarSystemTool
{
    [MenuItem("Tools/6. Finalize Orbits (Remove Animator Conflict)")]
    public static void Fix()
    {
        GameObject solarTarget = GameObject.Find("ImageTarget_SolarSystem");
        if (solarTarget == null) return;

        // Find the container
        Transform container = solarTarget.transform.Find("ScaleWrapper/Tata Surya Albi try fix");
        if (container == null) container = solarTarget.transform.Find("Tata Surya Albi try fix");

        if (container != null)
        {
            // The Unity Animator is overriding the X and Z positions to (0,0,0)!
            // This is making the Earth and Moon teleport INSIDE the Sun.
            // By completely removing the Animator, we let our custom Orbit.cs scripts take full control!
            Animator anim = container.GetComponent<Animator>();
            if (anim != null)
            {
                Object.DestroyImmediate(anim);
            }

            // Also reset their positions just in case the animator permanently moved them
            Transform sun = container.Find("Sun");
            Transform earth = container.Find("Earth");
            Transform moon = container.Find("Moon");

            if (sun != null) sun.localPosition = Vector3.zero;
            if (earth != null) earth.localPosition = new Vector3(-2.24f, 0, 0);
            if (moon != null) moon.localPosition = new Vector3(-3.44f, 0.25f, 0);

            // Make sure the Orbit scripts are intact
            if (sun != null && sun.GetComponent<Orbit>() == null) sun.gameObject.AddComponent<Orbit>();
            if (earth != null && earth.GetComponent<Orbit>() == null) earth.gameObject.AddComponent<Orbit>();
            if (moon != null && moon.GetComponent<Orbit>() == null) moon.gameObject.AddComponent<Orbit>();
        }

        UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
        Debug.Log("Animator removed from Solar System. Earth and Moon have been physically separated from the Sun and will now Orbit perfectly!");
    }
}
