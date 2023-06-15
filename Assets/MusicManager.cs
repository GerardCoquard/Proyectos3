using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    private void Awake() {
        if(instance == null) instance = this;
        else Destroy(this);
    }
    public float fadeInSpeed;
    public float fadeOutSpeed;
    Dictionary<string,AudioSourceHandler> dictionary = new Dictionary<string, AudioSourceHandler>();
    public void AddSong(string soundName, float vol)
    {
        if(dictionary.ContainsKey(soundName)) return;
        dictionary.Add(soundName,AudioManager.Play(soundName).Loop(true).FadeIn(fadeInSpeed,vol));
    }
    public void StopSong(string soundName)
    {
        if(!dictionary.ContainsKey(soundName)) return;
        dictionary[soundName].FadeOut(fadeOutSpeed);
        dictionary.Remove(soundName);
    }
}
