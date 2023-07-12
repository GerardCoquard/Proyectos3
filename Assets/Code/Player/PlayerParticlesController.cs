using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticlesController : MonoBehaviour
{
    public GameObject landParticles;
    public Transform footTransform;

    public void PlayLandParticles()
    {
        Instantiate(landParticles, footTransform.position,landParticles.transform.rotation);
    }
}
