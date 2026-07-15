using UnityEngine;
using UnityEditor;

public class FixSolarSystemInteractableTool
{
    [MenuItem("Tools/Fix Solar System Title")]
    public static void FixTitle()
    {
        GameObject solarSystem = GameObject.Find("ImageTarget_SolarSystem");
        if (solarSystem != null)
        {
            // First, it requires a collider, so let's add a dummy box collider that encompasses everything
            // WE MUST DO THIS FIRST before adding PlanetInteractable!
            BoxCollider col = solarSystem.GetComponent<BoxCollider>();
            if (col == null)
            {
                col = solarSystem.AddComponent<BoxCollider>();
                col.size = new Vector3(10, 10, 10); // large enough to cover the system
            }

            PlanetInteractable pi = solarSystem.GetComponent<PlanetInteractable>();
            if (pi == null)
            {
                pi = solarSystem.AddComponent<PlanetInteractable>();
            }

            pi.planetName = "Solar System";
            pi.generalInformation = "The Solar System is the gravitationally bound system of the Sun and the objects that orbit it. It formed 4.6 billion years ago from the gravitational collapse of a giant interstellar molecular cloud.";
            
            // Also find the animator (which is on the child 'Tata Surya Albi try fix')
            foreach (Transform child in solarSystem.transform)
            {
                Animator anim = child.GetComponent<Animator>();
                if (anim != null)
                {
                    pi.animator = anim;
                    break;
                }
            }

            UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
            EditorUtility.DisplayDialog("Fixed", "The Solar System marker will now correctly display 'Solar System' instead of 'Earth' or 'Sun'!", "OK");
        }
    }
}
