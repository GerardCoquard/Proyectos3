using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookRandomizer : MonoBehaviour
{
    public List<Material> materials;
    public Renderer _renderer;
    private void Start() {
        _renderer.material = materials[Random.Range(0,materials.Count)];
    }
}
