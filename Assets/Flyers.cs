using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flyers : MonoBehaviour
{
    public float fadeSpeed;
    List<Image> flyers;
    private void Awake() {
        flyers = new List<Image>(GetComponentsInChildren<Image>());
    }
    private void OnEnable() {
        foreach (Image item in flyers)
        {
            item.color = new Color(1,1,1,0);
        }
    }
    public void SetFlyers(bool state)
    {
        if(flyers.Count <= 0) flyers = new List<Image>(GetComponentsInChildren<Image>());
        StopAllCoroutines();
        if(state) StartCoroutine(FadeIn());
        else StartCoroutine(FadeOut());
    }
    IEnumerator FadeIn()
    {
        while(flyers[0].color.a < 1)
        {
            foreach (Image item in flyers)
            {
                item.color = new Color(1,1,1,item.color.a + fadeSpeed * Time.unscaledDeltaTime);
            }
            yield return null;
        }
        foreach (Image item in flyers)
        {
            item.color = new Color(1,1,1,1);
        }
    }
    IEnumerator FadeOut()
    {
        while(flyers[0].color.a > 0)
        {
            foreach (Image item in flyers)
            {
                item.color = new Color(1,1,1,item.color.a - fadeSpeed * Time.unscaledDeltaTime);
            }
            yield return null;
        }
        foreach (Image item in flyers)
        {
            item.color = new Color(1,1,1,0);
        }
    }
}
