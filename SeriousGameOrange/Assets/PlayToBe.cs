using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PlayToBe : MonoBehaviour
{
    public AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null && audioSource.clip != null)
        {
            Debug.Log("Starting audio playback...");
            StartCoroutine(WaitForSoundToEnd());
        }
        else
        {
            Debug.LogError("AudioSource or AudioClip is missing!");
        }
    }

    IEnumerator WaitForSoundToEnd()
    {
        if (audioSource.clip.length > 0)
        {
            audioSource.Play();
            Debug.Log("Audio playing for " + audioSource.clip.length + " seconds...");
            yield return new WaitForSeconds(audioSource.clip.length);
            Debug.Log("Audio finished. Loading scene...");
            SceneManager.LoadScene(0);  // Replace 0 with the actual scene index or name
        }
        else
        {
            Debug.LogError("AudioClip has no length!");
        }
    }
}
