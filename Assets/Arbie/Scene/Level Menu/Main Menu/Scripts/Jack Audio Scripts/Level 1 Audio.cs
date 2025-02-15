using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSoundManager : MonoBehaviour
{
    public AudioClip soundClip;
    private AudioSource audioSource;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Game Scene") // Replace with your scene name
        {
            AudioManager.instance.Play("L1Theme");
        }
    }
}