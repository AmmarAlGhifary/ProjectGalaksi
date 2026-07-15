using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class PlanetInteractable : MonoBehaviour
{
    [Header("Planet Data")]
    public string planetName;
    [TextArea(3, 10)]
    public string generalInformation;
    public AudioClip narrationAudio;
    
    [Header("Quiz Data")]
    public List<QuizQuestion> specificQuizQuestions;


    [Header("Animation Setup")]
    public Animator animator;
    public string actionAnimationTrigger = "PlayAction";

    [Tooltip("If set, the slider will scale this object instead of the one the script is attached to. Useful for bypassing Animator scale fighting.")]
    public Transform targetToScale;

    // Store initial transform data for slider manipulation
    private Vector3 initialScale;
    private Quaternion initialRotation;

    void Start()
    {
        if (targetToScale == null) targetToScale = transform;
        initialScale = targetToScale.localScale;
        initialRotation = transform.localRotation;

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    public void UpdateTransformFromSlider(float sliderValue)
    {
        if (targetToScale == null) targetToScale = transform;

        // Slider value is assumed to be 0 to 1
        // Scale from 20% to 200% of initial scale
        float scaleModifier = Mathf.Lerp(0.2f, 2.0f, sliderValue);
        targetToScale.localScale = initialScale * scaleModifier;

        // Rotation is now handled purely by the Animation Controller!
    }

    public void PlayActionAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger(actionAnimationTrigger);
        }
    }
}
