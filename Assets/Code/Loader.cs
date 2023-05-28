using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    public static Loader instance;
    [SerializeField] GameObject loadingScreen;
    private float progress;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) { LoadScene("JAN_Scene"); }
    }
    public void LoadScene(string scene)
    {
        StartCoroutine(LoadSceneAsync(scene));
    }

    IEnumerator LoadSceneAsync(string scene)
    {

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);

        loadingScreen.SetActive(true);

        while (!asyncOperation.isDone)
        {
            progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            yield return null;
        }
    }
    public float GetLoadingProgress()
    {
        return progress;
    }


}
