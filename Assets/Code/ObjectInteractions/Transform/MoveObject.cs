﻿using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MoveObject : MonoBehaviour
{
    [SerializeField] Transform finalPosition;
    [SerializeField] float timeToReach;
    [SerializeField] float delay;
    [SerializeField] ParticleSystem onMoveParticles;
    [SerializeField] float timeForParticles;
    Vector3 initPos;
    Vector3 finalPos;
    float distanceBetweenPositions;
    bool locked;
    public UnityEvent OnStart;
    public UnityEvent OnFinish;

    private void Start()
    {
        initPos = transform.position;
        finalPos = finalPosition.position;
        distanceBetweenPositions = Vector3.Distance(initPos, finalPos);
    }
    public void ChangeParams(Transform final, float newTime)
    {
        timeToReach = newTime;
        finalPos = final.position;
        distanceBetweenPositions = Vector3.Distance(initPos, finalPos);
    }
    public void Move()
    {
        if(locked) return;
        StopAllCoroutines();
        StartCoroutine(MoveCoroutine());
        if(onMoveParticles != null) StartCoroutine(PlayParticlesWhileMoving());
    }
    public void ResetMove()
    {
        if(locked) return;
        StopAllCoroutines();
        StartCoroutine(ResetObjectCoroutine());
    }

    IEnumerator PlayParticlesWhileMoving()
    {
        float timer = 0f;
        while(timer <= timeForParticles)
        {
            timer += Time.deltaTime;
            if (!onMoveParticles.isPlaying) onMoveParticles.Play();
            yield return null;
        }
    }
    IEnumerator MoveCoroutine()
    {
        OnStart?.Invoke();
        yield return new WaitForSeconds(delay);
        float distanceToTarget = Vector3.Distance(transform.position, finalPos);
        float time = distanceToTarget / distanceBetweenPositions * timeToReach;
        Vector3 _initPos = transform.position;
        float timer = 0f;
        while (timer<time)
        {
            transform.position = Vector3.Lerp(_initPos, finalPos, timer / time);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.position = finalPos;
        OnFinish?.Invoke();
    }
    IEnumerator ResetObjectCoroutine()
    {
        OnStart?.Invoke();
        float distanceToTarget = Vector3.Distance(transform.position, initPos);
        float time = distanceToTarget / distanceBetweenPositions * timeToReach;
        Vector3 _initPos = transform.position;
        float timer = 0f;
        while (timer<time)
        {
            transform.position = Vector3.Lerp(_initPos, initPos, timer / time);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.position = initPos;
        OnFinish?.Invoke();
    }
    public void SetLocked(bool state)
    {
        locked = state;
    }
}
