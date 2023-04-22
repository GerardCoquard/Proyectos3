using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OptionsDefaultConfig", menuName = "Options Default", order = 1)]
public class OptionsDefaultConfig : ScriptableObject
{
    public bool cameraShake = true;
    public bool cursorLock = true;
    public bool fullScreen = true;
    public bool subtitles = true;
    public bool vSync = true;
    public int resolution = -1;
}
