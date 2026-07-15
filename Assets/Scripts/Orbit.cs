using UnityEngine;

public class Orbit : MonoBehaviour
{
    [Header("Orbit Settings")]
    [Tooltip("The object to orbit around. If null, it will only rotate in place.")]
    public Transform target;
    public float orbitSpeed = 10f;
    
    [Header("Rotation Settings")]
    public float rotationSpeed = 20f;
    public Vector3 axis = Vector3.up;

    void Update()
    {
        // Orbit around the target if one is set
        if (target != null)
        {
            transform.RotateAround(target.position, axis, orbitSpeed * Time.deltaTime);
        }

        // Spin on its own axis
        transform.Rotate(axis, rotationSpeed * Time.deltaTime, Space.Self);
    }
}
