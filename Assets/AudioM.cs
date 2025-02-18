using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    public AudioMixerGroup mixerGroup;

    public Sound[] sounds;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;

            s.source.outputAudioMixerGroup = mixerGroup;
        }
    }
            

    public void Play(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

        s.source.Play();
    }

    public void Stop(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }
        if (s.source.isPlaying)
        {
            s.source.Stop();
        }
    }
    public bool IsPlaying(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return false;
        }

        return s.source.isPlaying;
    }
    public void PlaySceneMusic(string sceneName)
    {
        string musicToPlay = "";

        switch (sceneName)
        {
            case "MainMenu":
            case "LevelSelection": // Ensure LevelSelection uses Menu music
                musicToPlay = "Menu";
                break;
            case "GameScene":
                musicToPlay = "Practice Level Theme";
                break;
            case "Level 1":
                musicToPlay = "Level 1 Theme";
                break;
            case "Level 2":
                musicToPlay = "Level 2 Theme";
                break;
        }

        if (!string.IsNullOrEmpty(musicToPlay))
        {
            // Stop all sounds before playing new music
            StopAll();

            // Check if the music is already playing before playing it again
            if (!IsPlaying(musicToPlay))
            {
                Play(musicToPlay);
            }
        }
    }
    public void StopAll()
    {
        foreach (var sound in sounds)
        {
            if (sound.source.isPlaying)
            {
                Debug.Log("Stopping sound: " + sound.name);
                sound.source.Stop();
            }
        }
    }
    private void StopAllMusic()
    {
        foreach (Sound s in sounds)
        {
            if (s.source.isPlaying)
            {
                s.source.Stop();
            }
        }
    }
}
