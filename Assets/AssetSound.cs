using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetSound : MonoBehaviour
{
    public string soundName = "televisionAudio";
    public float volume = 1f;
    public bool loop;
    private void Start()
    {
        AudioManager.Play(soundName).SpatialBlend(transform.position, 15f).Volume(volume).Loop(loop);
    }
}
