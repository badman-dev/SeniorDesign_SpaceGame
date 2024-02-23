using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] buttonSounds;

    private void Start()
    {
        if (!audioSource)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void playButtonSound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void playRandomButtonSound()
    {
        if (buttonSounds.Length != 0)
        {
            audioSource.PlayOneShot(buttonSounds[Random.Range(0, buttonSounds.Length)]);
        }
    }
}
