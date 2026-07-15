using UnityEngine;
using UnityEditor;

public class SetupOrbitTool
{
    [MenuItem("Tools/4. Enable Orbit Animation")]
    public static void EnableOrbit()
    {
        GameObject solarTarget = GameObject.Find("ImageTarget_SolarSystem");
        if (solarTarget == null)
        {
            Debug.LogError("ImageTarget_SolarSystem not found!");
            return;
        }

        Transform container = solarTarget.transform.Find("Tata Surya Albi try fix");
        if (container == null)
        {
            Debug.LogError("Container not found!");
            return;
        }

        Transform sun = container.Find("Sun");
        Transform earth = container.Find("Earth");
        Transform moon = container.Find("Moon");

        if (sun != null)
        {
            Orbit sunOrbit = sun.GetComponent<Orbit>();
            if (sunOrbit == null) sunOrbit = sun.gameObject.AddComponent<Orbit>();
            sunOrbit.target = null; // Sun just spins in place
            sunOrbit.rotationSpeed = 15f;
            sunOrbit.orbitSpeed = 0f;
        }

        if (earth != null && sun != null)
        {
            Orbit earthOrbit = earth.GetComponent<Orbit>();
            if (earthOrbit == null) earthOrbit = earth.gameObject.AddComponent<Orbit>();
            earthOrbit.target = sun; // Earth orbits the Sun
            earthOrbit.rotationSpeed = 45f;
            earthOrbit.orbitSpeed = 30f;
        }

        if (moon != null && earth != null)
        {
            Orbit moonOrbit = moon.GetComponent<Orbit>();
            if (moonOrbit == null) moonOrbit = moon.gameObject.AddComponent<Orbit>();
            moonOrbit.target = earth; // Moon orbits the Earth
            moonOrbit.rotationSpeed = 60f;
            moonOrbit.orbitSpeed = 90f; // Moon orbits faster
        }

        UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
        Debug.Log("Orbit animations applied successfully! The Earth and Moon will now orbit dynamically.");
    }
}
