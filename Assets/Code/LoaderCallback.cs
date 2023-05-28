using UnityEngine;

public class LoaderCallback : MonoBehaviour
{
    bool isFirstUpdate = true;
    private void Start()
    {
        Loader.LoaderCallback();
    }
}
