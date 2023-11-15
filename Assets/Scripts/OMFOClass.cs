using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class OMFOClass : MonoBehaviour
{
    private static OMFOClass instance;  // Singleton instance

    private AudioSource _audioSource;

    void Awake()
    {
        // Implement the singleton pattern

        Destroy(gameObject);

        _audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        // Subscribe to the scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Unsubscribe from the scene loaded event to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Stop music when a new scene is loaded
        StopMusic();

        // Start music for the current scene
        PlayMusic();
    }

    public void PlayMusic()
    {
        _audioSource.Play();
    }

    public void StopMusic()
    {
        _audioSource.Stop();
    }
}