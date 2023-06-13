using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventMonsterMirror : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] LineRenderer mirrorLineRenderer;
    [SerializeField] Transform eyeTransform;
    [SerializeField] Transform mirrorEyeTransform;
    [SerializeField] Transform mirrorReciver;
    [SerializeField] List<ParticleSystem> chargeParticles;
    public float finalParticleScale;
    public float scaleRatio;
    public float delayToKill;
    public float delayToDisapear;
    public float chargeTime;
    private bool isFinish;
    public UnityEvent eventToDo;

    private void Start()
    {
        StopParticles();
        isFinish = false;
        lineRenderer.gameObject.SetActive(false);
    }

    private void PlayParticles()
    {
        foreach (ParticleSystem particle in chargeParticles)
        {
            particle.Play();
        }
    }

    private void StopParticles()
    {
        foreach (ParticleSystem particle in chargeParticles)
        {
            particle.Stop();
        }
    }

   
    public void ThrowToMonster()
    {
       
        //Original
        lineRenderer.gameObject.SetActive(true);
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, eyeTransform.position);
        //Mirror
        mirrorLineRenderer.gameObject.SetActive(true);
        mirrorLineRenderer.SetPosition(0, mirrorReciver.position);
        mirrorLineRenderer.SetPosition(1, mirrorEyeTransform.position);

        StartCoroutine(EventMonster());
        isFinish = true;
    }

    public void StartMonsterEvent()
    {
        if (isFinish) return;
        StartCoroutine(ChargeLaser());
    }
    IEnumerator ChargeLaser()
    {
        float localScale = chargeParticles[0].transform.localScale.x;
        PlayParticles();
        while(chargeParticles[0].transform.localScale.x < finalParticleScale)
        {
            localScale += scaleRatio * Time.deltaTime;
            chargeParticles[0].transform.localScale = new Vector3(localScale, localScale, localScale);
        }
        chargeParticles[0].transform.localScale = new Vector3(finalParticleScale, finalParticleScale, finalParticleScale);
        yield return new WaitForSeconds(chargeTime);
        StopParticles();
        ThrowToMonster();
    }

    

    IEnumerator EventMonster()
    {
        yield return new WaitForSeconds(delayToKill);
        eventToDo?.Invoke();
        yield return new WaitForSeconds(delayToDisapear);
        lineRenderer.gameObject.SetActive(false);
        mirrorLineRenderer.gameObject.SetActive(false);
        eyeTransform.gameObject.SetActive(false);
        mirrorEyeTransform.gameObject.SetActive(false);
    }

}
