using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{

    private static AsyncOperation asyncOperation;
    private static Action onLoaderCallback;

    private class LoadingMonoBehaviour : MonoBehaviour { };

    public static void LoadScene(string scene)
    {
        onLoaderCallback = () =>
        {
            GameObject loadingGameObject = new GameObject("Loading Game Object");
            loadingGameObject.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAsync(scene));
        };

        SceneManager.LoadScene("LoadingScene");

    }

    static IEnumerator LoadSceneAsync(string scene)
    {
        yield return null;
        
        asyncOperation = SceneManager.LoadSceneAsync(scene);


        while (!asyncOperation.isDone)
        { 
            yield return null;
        }
    }
    public static float GetLoadingProgress()
    {
        if (asyncOperation != null)
        {
            
            return asyncOperation.progress;
        }
        else
        {
            return 0f;
        }
    }

    public static void LoaderCallback()
    {
        if (onLoaderCallback != null)
        {
            onLoaderCallback();
            onLoaderCallback = null;
        }
    }
}
