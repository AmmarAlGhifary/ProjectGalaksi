using UnityEngine;

public class SolarSystemController : MonoBehaviour
{
    public void PlayAllAnimations()
    {
        Orbit[] allOrbits = GetComponentsInChildren<Orbit>(true);
        foreach (Orbit orbit in allOrbits)
        {
            orbit.enabled = true;
        }
        Debug.Log("[SolarSystemController] Resumed all planetary orbits.");
    }

    public void StopAllAnimations()
    {
        Orbit[] allOrbits = GetComponentsInChildren<Orbit>(true);
        foreach (Orbit orbit in allOrbits)
        {
            orbit.enabled = false;
        }
        Debug.Log("[SolarSystemController] Paused all planetary orbits.");
    }
}
