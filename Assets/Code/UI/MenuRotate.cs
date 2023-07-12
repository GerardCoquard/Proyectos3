using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuRotate : MonoBehaviour
{
    public float minTime;
    public float maxTime;
    public RotatingPlatform platform;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim.Play("Pause");
        anim.SetBool("Paused",true);
        StartCoroutine(RotateIn(Random.Range(minTime,maxTime)));
    }
    IEnumerator RotateIn(float time)
    {
        float timer = 0;
        while(timer<time)
        {
            timer+=Time.deltaTime;
            yield return null;
        }
        platform.RotatePlatformLeft();
        StartCoroutine(RotateIn(Random.Range(minTime,maxTime)));
    }
}
