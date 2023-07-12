using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookAnimationRandomizer : MonoBehaviour
{
    Animation animationController;
    [SerializeField] List<AnimationClip> clips;

    private void Start()
    {
        animationController = GetComponent<Animation>();
    }

    public void PlayRandomAnimation()
    {
        animationController.Play(clips[Random.Range(0, clips.Count)].name);
    }
   
}
