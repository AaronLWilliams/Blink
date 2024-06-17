using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;

    public AudioSource audioSource;
    public AudioClip mainMenuMusic;
    //public AudioClip levelMusic;

    void Awake()
    {
        // Ensure that there is only one instance of the MusicManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // This will prevent the object from being destroyed on scene load
            SceneManager.sceneLoaded += OnSceneLoaded; 
        }
        else
        {
            Destroy(gameObject); // If another instance exists, destroy this one
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Change the music based on the scene name
        if (scene.name == "MainMenu" || scene.name == "LevelSelect")
        {
            PlayMusic(mainMenuMusic);
        }
        else if (scene.name.Contains("Level"))
        {
            //PlayMusic(levelMusic);
        }
    }

    void PlayMusic(AudioClip clip)
    {
        if (audioSource.clip != clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
