using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlParticles : MonoBehaviour
{
    [SerializeField] List<ParticleSystem> particlesList;

    public void PlayParticles()
    {
        foreach (ParticleSystem p in particlesList)
        {
            p.Play();
        }
    }
    public void StopParticles()
    {
        foreach (ParticleSystem p in particlesList)
        {
            p.Stop();
        }
    }
}
