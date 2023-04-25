using UnityEngine;

public class LightReflectorObject : MonoBehaviour
{

    [SerializeField] Light myLight;
    private bool refractionEnabled;

    private void Update()
    {
        myLight.gameObject.SetActive(refractionEnabled);
        refractionEnabled = true;
    }

    public void CreateRefraction(Vector3 entryDirection, Vector3 hitNormal)
    {
        if (refractionEnabled)
            return;

        Vector3 reflectedDirection = Vector3.Reflect(entryDirection, hitNormal);
        refractionEnabled = true;
        myLight.StartLight(reflectedDirection);
    }

}

