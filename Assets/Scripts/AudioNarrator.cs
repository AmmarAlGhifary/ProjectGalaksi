using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioNarrator : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayNarration(AudioClip clip)
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    public void StopNarration()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
