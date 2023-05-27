using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    public static Loader instance;
    private AsyncOperation asyncOperation;
    private static Action onLoaderCallback;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) { LoadScene("JAN_Scene"); }
    }
    private void Awake()
    {
        if(instance == null) { instance = this; } 
        else { Destroy(this); }
        DontDestroyOnLoad(this);
    }

    public void LoadScene(string scene)
    {
        onLoaderCallback = () =>
        {
            StartCoroutine(LoadSceneAsync(scene));
        };

        SceneManager.LoadScene("LoadingScene");

    }

    public float GetLoadingProgress()
    {
        if(asyncOperation != null)
        {
            return asyncOperation.progress;
        }
        else
        {
            return 1f;
        }
    }
    IEnumerator LoadSceneAsync(string scene)
    {
        yield return null;

        asyncOperation = SceneManager.LoadSceneAsync(scene);


        while (!asyncOperation.isDone)
        {
            yield return null;
        }
    }

    public void LoaderCallback()
    {
        if(onLoaderCallback != null)
        {
            onLoaderCallback();
            onLoaderCallback = null;
        }
    }
}
