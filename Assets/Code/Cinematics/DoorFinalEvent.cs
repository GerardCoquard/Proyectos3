using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorFinalEvent : MonoBehaviour
{
    public float speed = 2f;
    public Image bg;
    
    public void FadeOut()
    {
        StartCoroutine(FadeOutCo());
    }

    IEnumerator FadeOutCo()
    {
        float alpha = 0f;
        while(bg.color.a < 1f)
        {
            alpha += Time.deltaTime * speed;
            bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, alpha);
            yield return null;
        }
        bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, 1);
        Loader.instance.LoadScene("END_CINEMATIC");
    }
}
