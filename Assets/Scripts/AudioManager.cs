using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour {
    
    [Range(0f, 1f)]
    public float master_volume = 1.0f;

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
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
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
            s.source.volume = s.volume * AudioManager.instance.master_volume;
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
