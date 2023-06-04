using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinematic : MonoBehaviour
{
    public float delay;
    public Transform target;
    public float time;
    public bool oneShot;
    bool activated;
    public void PlayCinematic()
    {
        if(oneShot && activated) return;
        activated = true;
        StartCoroutine(Delay());
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(delay);
        CameraController.instance.Cinematic(target==null?transform:target,time);
    }
}
