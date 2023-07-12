using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeEventAnimations : MonoBehaviour
{
    public Animation eyeAnim;
    public Animation picAnim;


    public void PlayEyeAnim()
    {
        eyeAnim.Play();
    }

    public void PlayPicAnim()
    {
        picAnim.Play();
    }
}
