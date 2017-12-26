using UnityEngine;
using UnityEngine.Audio;

public enum SoundChannel
{
    Master,
    Music,
    SFX,

    Count__
}

[System.Serializable]
public class Sound
{
    public string name;

    public SoundChannel channel;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}
