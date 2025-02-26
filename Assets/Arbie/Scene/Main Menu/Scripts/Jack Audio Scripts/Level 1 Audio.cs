using UnityEngine;

public class PlaySoundInScene : MonoBehaviour
{
    public AudioClip soundClip; // Assign this in the Inspector
    private AudioSource audioSource;

    void Start()
    {
        // Get or add an AudioSource component
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Set the audio clip and play it
        audioSource.clip = soundClip;
        audioSource.playOnAwake = false; // Prevent auto-play on load
        audioSource.Play();
    }
}
