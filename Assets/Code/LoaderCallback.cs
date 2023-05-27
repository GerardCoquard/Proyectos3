using UnityEngine;

public class LoaderCallback : MonoBehaviour
{
    bool isFirstUpdate = true;
    private void Update()
    {
        if (isFirstUpdate)
        {
            isFirstUpdate = false;
            Loader.LoaderCallback();
            
        }
    }
}
