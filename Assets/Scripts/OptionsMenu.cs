using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour {

    public delegate void SoundChanged();
    public static event SoundChanged OnSoundChanged;
    
	public void SetMasterVolume (float volume)
    {
        AudioManager.instance.master_volume = volume;

        if (OnSoundChanged != null)
            OnSoundChanged();
    }
}
