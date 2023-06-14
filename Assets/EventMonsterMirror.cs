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
    [SerializeField] Transform startPos;
    [SerializeField] List<ParticleSystem> chargeParticles;
    public float finalParticleScale;
    public float scaleRatio;
    public float delayToKill;
    public float monsterHitDelay;
    public float delayToDisapear;
    public float chargeTime;
    private bool isFinish;
    public UnityEvent eventToDo;
    public UnityEvent beforeEventToDo;
    public UnityEvent eventWhenDisapear;

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
        lineRenderer.SetPosition(0, startPos == null ? transform.position : startPos.position);
        lineRenderer.SetPosition(1, eyeTransform.position);
        //Mirror
        if(mirrorLineRenderer != null)
        {
            mirrorLineRenderer.gameObject.SetActive(true);
            mirrorLineRenderer.SetPosition(0, mirrorReciver.position);
            mirrorLineRenderer.SetPosition(1, mirrorEyeTransform.position);
        }
       

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
        Debug.Log("in");
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
        yield return new WaitForSeconds(monsterHitDelay);
        beforeEventToDo?.Invoke();
        yield return new WaitForSeconds(delayToKill);
        eventToDo?.Invoke();
        yield return new WaitForSeconds(delayToDisapear);
        eventWhenDisapear?.Invoke();
        lineRenderer.gameObject.SetActive(false);
        if (mirrorLineRenderer != null) mirrorLineRenderer.gameObject.SetActive(false);
       
        if (mirrorLineRenderer != null) mirrorEyeTransform.gameObject.SetActive(false);
    }

}
