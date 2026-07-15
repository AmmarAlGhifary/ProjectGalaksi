using UnityEngine;
using UnityEditor;

public class FinalSolarSystemFixTool
{
    [MenuItem("Tools/5. Ultimate Solar System Fix")]
    public static void FixIt()
    {
        GameObject solarTarget = GameObject.Find("ImageTarget_SolarSystem");
        if (solarTarget == null) return;

        Transform container = solarTarget.transform.Find("Tata Surya Albi try fix");
        if (container == null) return;

        // 1. Create a Scale Wrapper
        Transform scaleWrapper = solarTarget.transform.Find("ScaleWrapper");
        if (scaleWrapper == null)
        {
            GameObject wrapperObj = new GameObject("ScaleWrapper");
            scaleWrapper = wrapperObj.transform;
            scaleWrapper.SetParent(solarTarget.transform);
            scaleWrapper.localPosition = Vector3.zero;
            scaleWrapper.localRotation = Quaternion.identity;
        }

        // Set the wrapper scale to 0.2f so it's small enough to fit the screen
        scaleWrapper.localScale = new Vector3(0.2f, 0.2f, 0.2f);

        // 2. Move the container inside the wrapper
        container.SetParent(scaleWrapper);
        container.localPosition = Vector3.zero;
        container.localRotation = Quaternion.identity;
        container.localScale = Vector3.one; // The animator forces this to 1 anyway!

        // 3. Move PlanetInteractable to the wrapper
        PlanetInteractable oldPI = container.GetComponent<PlanetInteractable>();
        if (oldPI != null) Object.DestroyImmediate(oldPI);

        BoxCollider oldCol = container.GetComponent<BoxCollider>();
        if (oldCol != null) Object.DestroyImmediate(oldCol);

        BoxCollider newCol = scaleWrapper.gameObject.GetComponent<BoxCollider>();
        if (newCol == null)
        {
            newCol = scaleWrapper.gameObject.AddComponent<BoxCollider>();
            newCol.size = new Vector3(10, 10, 10);
        }

        PlanetInteractable newPI = scaleWrapper.gameObject.GetComponent<PlanetInteractable>();
        if (newPI == null) newPI = scaleWrapper.gameObject.AddComponent<PlanetInteractable>();

        newPI.planetName = "Solar System";
        newPI.generalInformation = "The Solar System is the gravitationally bound system of the Sun and the objects that orbit it.";
        
        // Link the animator from the child container
        Animator anim = container.GetComponent<Animator>();
        if (anim != null) newPI.animator = anim;

        // 4. Double check the Orbit scripts
        Transform sun = container.Find("Sun");
        Transform earth = container.Find("Earth");
        Transform moon = container.Find("Moon");
        
        if (sun != null)
        {
            Orbit sunOrbit = sun.GetComponent<Orbit>();
            if (sunOrbit == null) sunOrbit = sun.gameObject.AddComponent<Orbit>();
            sunOrbit.target = null;
            sunOrbit.rotationSpeed = 15f;
        }
        if (earth != null && sun != null)
        {
            Orbit earthOrbit = earth.GetComponent<Orbit>();
            if (earthOrbit == null) earthOrbit = earth.gameObject.AddComponent<Orbit>();
            earthOrbit.target = sun;
            earthOrbit.rotationSpeed = 45f;
            earthOrbit.orbitSpeed = 30f;
        }
        if (moon != null && earth != null)
        {
            Orbit moonOrbit = moon.GetComponent<Orbit>();
            if (moonOrbit == null) moonOrbit = moon.gameObject.AddComponent<Orbit>();
            moonOrbit.target = earth;
            moonOrbit.rotationSpeed = 60f;
            moonOrbit.orbitSpeed = 90f;
        }

        UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
        Debug.Log("Ultimate Fix Applied! The scale wrapper will prevent the Animator from making the Sun huge.");
    }
}
