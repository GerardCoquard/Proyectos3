using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyEvent : MonoBehaviour
{
    public Animation myAnim;
    public AnimationClip clip;


    public void PlayKeyAnim()
    {
        myAnim.Play(clip.name);
    }

    public void PlayUnlockSound()
    {
        AudioManager.Play("unlockSound").Volume(1f);
    }

    public void PlayDoorOpenSound()
    {
        AudioManager.Play("doorOpenSound").Volume(1f);

    }
}
