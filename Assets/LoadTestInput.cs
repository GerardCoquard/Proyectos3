using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadTestInput : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) { Loader.LoadScene("Lightning_Scene"); } 
    }
}
