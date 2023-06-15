using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTest : MonoBehaviour
{
    public string soundName;
    public float volume = 1;
    void Start()
    {
        AudioManager.Play(soundName).Loop(true).Volume(volume);
    }
}
