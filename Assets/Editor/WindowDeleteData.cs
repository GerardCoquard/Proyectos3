using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


public class WindowDeleteData : EditorWindow
{
    [MenuItem("Data/Delete Saved File")]
    public static void DeleteFile()
    {
        DataConfig config = Resources.Load<DataConfig>("DataConfig");
        if(config == null) return;
        File.Delete(Path.Combine(Application.persistentDataPath, config.GetFileName()));
    }
}

     