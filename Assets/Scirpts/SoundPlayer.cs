
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{
    [SerializeField] List<AudioClip> clips;
    [SerializeField] float pitchVariance;

    AudioSource source;
    float basePitch;
    private void Awake()
    {
        source = GetComponent<AudioSource>();
        basePitch = source.pitch;
    }

    public void Play()
    {
        AudioClip clip = clips[Random.Range(0,clips.Count)];
        source.pitch = basePitch + Random.Range(-pitchVariance, pitchVariance);
        source.PlayOneShot(clip);

    }

    public void SetVolume(float volume)
    {
        source.volume = volume;
    }
}
