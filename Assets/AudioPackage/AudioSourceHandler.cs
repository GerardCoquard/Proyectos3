using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSourceHandler : MonoBehaviour
{
    AudioSource audioSource;
    [NonSerialized] public bool dontPause = false;
    private void OnEnable() {
        audioSource = GetComponent<AudioSource>();
    }
    private void Update() {
        if(!audioSource.isPlaying && Time.timeScale != 0 && !audioSource.loop)
        {
            Destroy(gameObject);
        }
    }
    public AudioSourceHandler At(Vector3 pos)
    {
        gameObject.transform.position = pos;
        return this;
    }
    public AudioSourceHandler Volume(float vol)
    {
        audioSource.volume = vol;
        return this;
    }
    public AudioSourceHandler Loop(bool looped)
    {
        audioSource.loop = looped;
        return this;
    }
    public AudioSourceHandler Mute(bool muted)
    {
        audioSource.mute = muted;
        return this;
    }
    public AudioSourceHandler SpatialBlend(float sp)
    {
        sp = Mathf.Clamp(sp,0f,1f);
        audioSource.spatialBlend = sp;
        return this;
    }
    public AudioSourceHandler SpatialRadius(float min,float max)
    {
        audioSource.minDistance = min;
        audioSource.maxDistance = max;
        return this;
    }
    public AudioSourceHandler Pitch(float pitch)
    {
        audioSource.pitch = pitch;
        return this;
    }
    public AudioSourceHandler RandomPitch(float minPitch, float maxPitch)
    {
        float pitch = UnityEngine.Random.Range(minPitch,maxPitch);
        audioSource.pitch = pitch;
        return this;
    }
    public AudioSourceHandler NoPause(bool noPause)
    {
        dontPause = noPause;
        return this;
    }
    public void Pause()
    {
        if(!dontPause) audioSource.Pause();
    }
    public void UnPause()
    {
        if(!dontPause) audioSource.UnPause();
    }
    public void SetClip(AudioClip _clip)
    {
        audioSource.clip = _clip;
    }
    public void SetGroup(AudioMixerGroup _group)
    {
        audioSource.outputAudioMixerGroup = _group;
    }
    public void Play()
    {
        audioSource.Play();
    }
    public void Stop()
    {
        Destroy(gameObject);
    }
    public void FadeOut(float speed)
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutCoroutine(speed));
    }
    public void FadeIn(float speed, float maxVolume)
    {
        StopAllCoroutines();
        StartCoroutine(FadeInCoroutine(speed,maxVolume));
    }
    IEnumerator FadeOutCoroutine(float speed)
    {
        while(audioSource.volume > 0)
        {
            audioSource.volume-=speed*Time.unscaledDeltaTime;
            audioSource.volume = Mathf.Clamp(audioSource.volume,0f,1f);
            yield return null;
        }
        Destroy(gameObject);
    }
    IEnumerator FadeInCoroutine(float speed, float maxVolume)
    {
        audioSource.volume = 0;
        while(audioSource.volume < 1)
        {
            audioSource.volume+=speed*Time.unscaledDeltaTime;
            audioSource.volume = Mathf.Clamp(audioSource.volume,0f,1f);
            yield return null;
        }
    }
}
