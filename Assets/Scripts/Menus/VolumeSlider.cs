using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour {

    public SoundChannel channel;
    
	void Start ()
    {
        float volume = 0.0f;
        switch (channel)
        {
            case SoundChannel.Master:
                volume = AudioManager.Instance.master_volume;
                break;
            case SoundChannel.Music:
                volume = AudioManager.Instance.music_volume;
                break;
            case SoundChannel.SFX:
                volume = AudioManager.Instance.sfx_volume;
                break;
            default:
                break;
        }
        GetComponent<Slider>().value = volume;
    }
}
