using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TillingMaterial : MonoBehaviour
{
    [SerializeField] private float tileX = 1;
    [SerializeField] private float tileY = 1;
    Mesh mesh;
    Renderer _renderer;
    private MaterialPropertyBlock propertyBlock;
    private void OnValidate() {
        if(propertyBlock==null) propertyBlock = new MaterialPropertyBlock();
        mesh = GetComponent<MeshFilter>().mesh;
        _renderer = GetComponent<Renderer>();
        Vector2 scale = new Vector2((mesh.bounds.size.x * transform.localScale.x)/100*tileX, (mesh.bounds.size.y * transform.localScale.y)/100*tileY);
        propertyBlock.SetVector("_MainTex_ST", new Vector4(scale.x, scale.y, 0, 0));
        _renderer.SetPropertyBlock(propertyBlock);
    }
}
