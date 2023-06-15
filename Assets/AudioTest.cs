using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Play("bookModeOff").Loop(true);
    }
}
