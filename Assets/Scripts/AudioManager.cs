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

    public static AudioManager instance;

	void Awake ()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

		foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            // Set the source Volume depending on the channel
            switch (s.channel)
            {
                case SoundChannel.Music:
                    s.source.volume = s.volume * music_volume;
                    break;
                case SoundChannel.SFX:
                    s.source.volume = s.volume * sfx_volume;
                    break;
            }
        }        
	}

    void Start ()
    {
        PlaySound("MainTheme");
        OptionsMenu.OnSoundChanged += OnVolumeChange;
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

        s.source.volume = s.volume * master_volume;
        
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
