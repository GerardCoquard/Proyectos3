using UnityEngine;

public class LightReflectorObject : MonoBehaviour
{

    [SerializeField] Light myLight;
    private bool refractionEnabled;

    private void Update()
    {
        myLight.gameObject.SetActive(refractionEnabled);
        refractionEnabled = false;
    }

    public void CreateRefraction(Vector3 entryDirection, Vector3 hitPosition)
    {
        if (refractionEnabled)
            return;

        myLight.StartLight(entryDirection);
        refractionEnabled = true;
    }

}

