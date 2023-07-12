using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEmission : MonoBehaviour
{
    private MaterialPropertyBlock propertyBlock;
    private Renderer _renderer;
    private void Awake() {
        _renderer = GetComponentInChildren<Renderer>();
    }
    public void SetEmission(Color hdrColor)
    {
        if (propertyBlock == null) propertyBlock = new MaterialPropertyBlock();
        propertyBlock.SetColor("_EmissionColor",hdrColor);
        _renderer.SetPropertyBlock(propertyBlock);
    }
}
