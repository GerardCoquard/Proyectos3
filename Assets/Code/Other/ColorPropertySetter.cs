using UnityEngine;

public class ColorPropertySetter : MonoBehaviour
{
    private MaterialPropertyBlock propertyBlock;
    private Renderer _renderer;
    private const byte k_MaxByteForOverexposedColor = 191;
    private void Awake() {
        _renderer = GetComponentInChildren<Renderer>();
    }
    public void SetIntensity(Material material,float intensity)
    {
        if (propertyBlock == null) propertyBlock = new MaterialPropertyBlock();
        propertyBlock.SetColor("_EmissionColor", ChangeHDRColorIntensity(material.GetColor("_Color"),intensity));
        propertyBlock.SetColor("_Color", material.GetColor("_Color"));
        if(_renderer==null) _renderer = GetComponentInChildren<Renderer>();
        _renderer.SetPropertyBlock(propertyBlock);
    }
    private Color ChangeHDRColorIntensity(Color subjectColor, float intensityChange)
    {
        // Get color intensity
        float maxColorComponent = subjectColor.maxColorComponent;
        float scaleFactorToGetIntensity = k_MaxByteForOverexposedColor / maxColorComponent;
        float currentIntensity = Mathf.Log(255f / scaleFactorToGetIntensity) / Mathf.Log(2f);

        // Get original color with ZERO intensity
        float currentScaleFactor = Mathf.Pow(2, currentIntensity);
        Color originalColorWithoutIntensity = subjectColor / currentScaleFactor;

        // Set color intensity
        float newScaleFactor = Mathf.Pow(2, intensityChange);
        Color colorToRetun = originalColorWithoutIntensity * newScaleFactor;

        // Return color
        return colorToRetun;
    }
}
