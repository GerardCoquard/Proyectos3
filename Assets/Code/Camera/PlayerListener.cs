using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerListener : MonoBehaviour
{

    void Update()
    {
        transform.position = PlayerController.instance.transform.position + new Vector3(0, 1, 0);
    }
}
