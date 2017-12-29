using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour {

    [Range(0f, 1f)]
    public float master_volume = 1.0f;
    [Range(0f, 1f)]
    public float music_volume = 1.0f;
    [Range(0f, 1f)]
    public float sfx_volume = 1.0f;

    public Sound[] sounds;

    public static AudioManager Instance { get; private set; }

    private string master_prefs_key = "MasterVolume";
    private string music_prefs_key = "MusicVolume";
    private string sfx_prefs_key = "SFXVolume";

    void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        GetPlayerPrefsVolume();

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.volume = s.volume * master_volume;

            // Set the source Volume depending on the channel
            switch (s.channel)
            {
                case SoundChannel.Music:
                    s.source.volume *= music_volume;
                    break;
                case SoundChannel.SFX:
                    s.source.volume *= sfx_volume;
                    break;
            }
        }        
	}

    void Start ()
    {
        PlaySound("MainTheme");
        OptionsMenu.OnSoundChanged += OnVolumeChange;
    }

    private void GetPlayerPrefsVolume ()
    {
        master_volume   = PlayerPrefs.GetFloat(master_prefs_key , 1.0f);
        music_volume    = PlayerPrefs.GetFloat(music_prefs_key  , 1.0f);
        sfx_volume      = PlayerPrefs.GetFloat(sfx_prefs_key    , 1.0f);
    }

    public void OnVolumeChange ()
    {
        foreach (Sound s in sounds)
        {
            float channel_volume = 1.0f;
            switch (s.channel)
            {
                case SoundChannel.Music:
                    channel_volume = music_volume;
                    break;
                case SoundChannel.SFX:
                    channel_volume = sfx_volume;
                    break;
                default:
                    break;
            }
            s.source.volume = s.volume * channel_volume * master_volume;
        }
    }

    public void PlaySound (string sound_name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == sound_name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + sound_name + " not found!");
            return;
        }
        
        if (s.source.isPlaying)
            // The problem with PlayOneShot is that you cannot Stop
            // the audio clip from playing until it's finished
            s.source.PlayOneShot(s.clip);
        else
            s.source.PlayDelayed(0f);
    }

    public void StopSound (string sound_name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == sound_name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + sound_name + " not found!");
            return;
        }

        s.source.Stop();
    }
}
