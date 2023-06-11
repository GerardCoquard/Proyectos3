using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticlesController : MonoBehaviour
{
    public GameObject landParticles;
    public Transform footTransform;

    public void PlayLandParticles()
    {
        Debug.Log("IN");
        Instantiate(landParticles, footTransform.position,landParticles.transform.rotation);
    }
}
