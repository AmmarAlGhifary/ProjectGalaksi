using UnityEngine;
using Vuforia;

/// <summary>
/// Attach this script to the ImageTarget GameObject.
/// It logs all Vuforia tracking events and controls what happens
/// when the marker is found or lost.
/// </summary>
public class ARTrackingLogger : MonoBehaviour
{
    private ObserverBehaviour observerBehaviour;

    void Start()
    {
        observerBehaviour = GetComponent<ObserverBehaviour>();
        
        if (observerBehaviour != null)
        {
            Debug.Log("[AR Logger] ObserverBehaviour found on: " + gameObject.name);
            Debug.Log("[AR Logger] Target name: " + observerBehaviour.TargetName);
            observerBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
        }
        else
        {
            Debug.LogError("[AR Logger] No ObserverBehaviour found on " + gameObject.name + "! Is this attached to the ImageTarget?");
        }

        // Log the children of ImageTarget (these are the 3D models that should appear)
        Debug.Log("[AR Logger] ImageTarget children count: " + transform.childCount);
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            Debug.Log("[AR Logger] Child " + i + ": " + child.name + 
                      " | Active: " + child.gameObject.activeSelf + 
                      " | Position: " + child.localPosition +
                      " | Scale: " + child.localScale);
        }

        // Check if Vuforia is initialized
        Debug.Log("[AR Logger] VuforiaBehaviour exists: " + (VuforiaBehaviour.Instance != null));
        if (VuforiaBehaviour.Instance != null)
        {
            Debug.Log("[AR Logger] VuforiaBehaviour enabled: " + VuforiaBehaviour.Instance.enabled);
        }
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus targetStatus)
    {
        Debug.Log("[AR Logger] === TARGET STATUS CHANGED ===");
        Debug.Log("[AR Logger] Target: " + behaviour.TargetName);
        Debug.Log("[AR Logger] Status: " + targetStatus.Status);
        Debug.Log("[AR Logger] StatusInfo: " + targetStatus.StatusInfo);

        if (targetStatus.Status == Status.TRACKED || 
            targetStatus.Status == Status.EXTENDED_TRACKED)
        {
            Debug.Log("[AR Logger] MARKER DETECTED! 3D models should be visible now!");
            Debug.Log("[AR Logger] ImageTarget world position: " + transform.position);
            
            // Log each child's state
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                Debug.Log("[AR Logger] Child '" + child.name + "' active: " + child.gameObject.activeSelf);
            }
        }
        else if (targetStatus.Status == Status.NO_POSE)
        {
            Debug.Log("[AR Logger] Marker LOST - camera can't see it anymore");
        }
    }

    void Update()
    {
        // Continuously log Vuforia state every 5 seconds
        if (Time.frameCount % 300 == 0)
        {
            if (VuforiaBehaviour.Instance != null)
            {
                Debug.Log("[AR Logger] Vuforia enabled: " + VuforiaBehaviour.Instance.enabled);
            }

            if (observerBehaviour != null)
            {
                Debug.Log("[AR Logger] Current target status: " + observerBehaviour.TargetStatus.Status + " -- " + observerBehaviour.TargetStatus.StatusInfo);
            }
        }
    }

    void OnDestroy()
    {
        if (observerBehaviour != null)
        {
            observerBehaviour.OnTargetStatusChanged -= OnTargetStatusChanged;
        }
    }
}
