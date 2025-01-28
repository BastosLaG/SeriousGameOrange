using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PlayToBe : MonoBehaviour
{
    public AudioSource audioSource;
    private AudioSource musicManager;
    void Start()
    {
        musicManager = GameObject.Find("MusicManager").GetComponent<AudioSource>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null && audioSource.clip != null)
        {
            StartCoroutine(WaitForSoundToEnd());
        }
    }

    IEnumerator WaitForSoundToEnd()
    {
        if (audioSource.clip.length > 0)
        {
            audioSource.Play();
            musicManager.Stop();
            yield return new WaitForSeconds(audioSource.clip.length);
            SceneManager.LoadScene(0);
        }
    }
}
