using UnityEngine;

public class LightReflectorObject : MonoBehaviour
{

    [SerializeField] LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer.enabled = false;
    }

    public void StartReflection()
    {
        lineRenderer.enabled = true;
    }

}

