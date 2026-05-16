using UnityEngine;

public class SpeakerLoop : MonoBehaviour
{
    public AudioSource audioSource;       // Reference to the AudioSource
    public AudioClip[] audioClips;        // Array of audio clips to play
    public float delayBetweenClips = 1f;  // Optional delay between clips

    private int currentIndex = 0;

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioClips.Length > 0)
            StartCoroutine(PlayLoop());
    }

    private System.Collections.IEnumerator PlayLoop()
    {
        while (true)
        {
            audioSource.clip = audioClips[currentIndex];
            audioSource.Play();

            // Wait until clip finishes + optional delay
            yield return new WaitForSeconds(audioSource.clip.length + delayBetweenClips);

            // Move to next clip (loop back if at end)
            currentIndex = (currentIndex + 1) % audioClips.Length;
        }
    }
}
