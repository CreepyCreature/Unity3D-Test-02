using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour {

    public Sound[] sounds;

    public static AudioManager instance;

	// Use this for initialization
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
	
	public void PlaySound (string sound_name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == sound_name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + sound_name + " not found!");
            return;
        }

        //Debug.Log("PlaySound(" + sound_name + ")");

        //s.source.Play();
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
