using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OptionsManager
{
    public static OptionsDefaultConfig defaultData;
    public static bool cameraShake;
    public static bool cursorLock;
    public static bool fullScreen;
    public static bool subtitles;
    public static bool vSync;
    public static int resolution;
    public static int defaultResolution;
    static OptionsManager()
    {
        defaultData = Resources.Load<OptionsDefaultConfig>("OptionsDefaultConfig");
        cameraShake = DataManager.Load<bool>("cameraShake");
        cursorLock = DataManager.Load<bool>("cursorLock");
        fullScreen = DataManager.Load<bool>("fullScreen");
        subtitles = DataManager.Load<bool>("subtitles");
        vSync = DataManager.Load<bool>("vSync");
        resolution = DataManager.Load<int>("resolution");
        defaultResolution = DataManager.Load<int>("defaultResolution");

        DataManager.onSave += SaveData;
    }
    static void SaveData()
    {
        DataManager.Save("cameraShake",cameraShake);
        DataManager.Save("cursorLock",cursorLock);
        DataManager.Save("fullScreen",fullScreen);
        DataManager.Save("subtitles",subtitles);
        DataManager.Save("vSync",vSync);
        DataManager.Save("resolution",resolution);
        DataManager.Save("defaultResolution",defaultResolution);
    }
}
