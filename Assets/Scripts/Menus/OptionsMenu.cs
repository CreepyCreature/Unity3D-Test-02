using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {

    public delegate void SoundChanged();
    public static event SoundChanged OnSoundChanged;

    private string master_prefs_key = "MasterVolume";
    private string music_prefs_key  = "MusicVolume";
    private string sfx_prefs_key    = "SFXVolume";
        
	public void SetMasterVolume (float volume)
    {
        AudioManager.Instance.master_volume = volume;
        PlayerPrefs.SetFloat(master_prefs_key, volume);

        if (OnSoundChanged != null)
            OnSoundChanged();
    }

    public void SetMusicVolume (float volume)
    {
        AudioManager.Instance.music_volume = volume;
        PlayerPrefs.SetFloat(music_prefs_key, volume);

        if (OnSoundChanged != null)
            OnSoundChanged();
    }

    public void SetSFXVolume (float volume)
    {
        AudioManager.Instance.sfx_volume = volume;
        PlayerPrefs.SetFloat(sfx_prefs_key, volume);

        if (OnSoundChanged != null)
            OnSoundChanged();
    }
}
