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
    private string currentLevelMusic;

    public void PlaySceneMusic(string sceneName)
    {
        string musicToPlay = "";

        switch (sceneName)
        {
            case "MainMenu":
            case "LevelSelection":
                musicToPlay = "Menu";
                break;
            case "GameScene":
                musicToPlay = "Practice Level Theme";
                break;
            case "level1":
                musicToPlay = "Level 1 Theme";
                break;
            case "level2":
                musicToPlay = "level2";
                break;
            case "level3":
                musicToPlay = "level3";
                break;
            case "level4":
                musicToPlay = "level4";
                break;
            case "level5":
                musicToPlay = "level5";
                break;
            case "level6":
                musicToPlay = "Level 1 Theme";
                break;
            case "level7":
                musicToPlay = "level2";
                break;
            case "level8":
                musicToPlay = "level3";
                break;
            case "level9":
                musicToPlay = "level4";
                break;
            case "level10":
                musicToPlay = "level5";
                break;
            case "level11":
                musicToPlay = "level1";
                break;
            case "level12":
                musicToPlay = "level5";
                break;
            case "Credits":
                musicToPlay = "Menu";
                break;
        }

        if (!string.IsNullOrEmpty(musicToPlay))
        {
            StopAll(); // Stop any music before playing new one

            if (!IsPlaying(musicToPlay))
            {
                Play(musicToPlay);
                currentLevelMusic = musicToPlay; // Store the music of the current level
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
    public void Pause(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }
        if (s.source.isPlaying)
        {
            s.source.Pause();
        }
    }
    public void PauseLevelMusic()
    {
        if (!string.IsNullOrEmpty(currentLevelMusic))
        {
            Pause(currentLevelMusic);
        }
    }

    public void Resume(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }
        if (!s.source.isPlaying)
        {
            s.source.UnPause();
        }
    }
    public void PauseAll()
    {
        foreach (var sound in sounds)
        {
            if (sound.source.isPlaying)
            {
                sound.source.Pause();
            }
        }
    }
    public void ResumeLevelMusic()
    {
        if (!string.IsNullOrEmpty(currentLevelMusic))
        {
            Resume(currentLevelMusic);
        }
    }

    public void ResumeAll()
    {
        foreach (var sound in sounds)
        {
            sound.source.UnPause();
        }
    }


}
