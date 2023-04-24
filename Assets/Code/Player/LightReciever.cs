using UnityEngine;

public class LightReciever : MonoBehaviour
{
    private bool isRecivingLight = false;

    public void SetRecivingLight(bool state)
    {
        isRecivingLight = state;
    }
}

