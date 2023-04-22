using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuOptions : Menu
{
    public List<RectTransform> tittleButtons;
    List<ButtonScreen> buttonsScreens;
    public float sizeMultiplier;
    public float sizeSpeedPerSecond;
    public override void OnStart() {
        base.OnStart();
        buttonsScreens = new List<ButtonScreen>();
        foreach (RectTransform item in tittleButtons)
        {
            buttonsScreens.Add(item.GetComponent<ButtonScreen>());
        }
    }
    public override void OnEnable() {
        base.OnEnable();
        for (int i = 0; i < tittleButtons.Count; i++)
        {
            if(tittleButtons[i].gameObject != firstButton)
            {
                tittleButtons[i].sizeDelta = new Vector2(buttonsScreens[i].GetWidth(),buttonsScreens[i].GetHeight());
                tittleButtons[i].GetComponent<ButtonScreen>().screen.SetActive(false);
            }
            else
            {
                tittleButtons[i].sizeDelta = new Vector2(buttonsScreens[i].GetWidth()*sizeMultiplier,buttonsScreens[i].GetHeight()*sizeMultiplier);
                tittleButtons[i].GetComponent<ButtonScreen>().screen.SetActive(true);
            }
        }
    }
    public void SetSelected(RectTransform button)
    {
        StopAllCoroutines();
        for (int i = 0; i < tittleButtons.Count; i++)
        {
            if(tittleButtons[i] != button)
            {
                StartCoroutine(Reduce(buttonsScreens[i]));
                buttonsScreens[i].screen.SetActive(false);
            }
            else
            {
                StartCoroutine(Grow(buttonsScreens[i]));
                buttonsScreens[i].screen.SetActive(true);
            }
        }
    }
    IEnumerator Reduce(ButtonScreen button)
    {
        RectTransform rect = button.GetRect();
        float _width = button.GetWidth();
        float _height = button.GetHeight();
        while(rect.sizeDelta.x > _width || rect.sizeDelta.y > _height)
        {
            rect.sizeDelta -= new Vector2(_width*sizeSpeedPerSecond*Time.unscaledDeltaTime,_height*sizeSpeedPerSecond*Time.unscaledDeltaTime);
            yield return null;
        }
        rect.sizeDelta = new Vector2(_width,_height);
    }
    IEnumerator Grow(ButtonScreen button)
    {
        RectTransform rect = button.GetRect();
        float _width = button.GetWidth();
        float _height = button.GetHeight();
        while(rect.sizeDelta.x < _width*sizeMultiplier || rect.sizeDelta.y < _height*sizeMultiplier)
        {
            rect.sizeDelta += new Vector2(_width*sizeSpeedPerSecond*Time.unscaledDeltaTime,_height*sizeSpeedPerSecond*Time.unscaledDeltaTime);
            yield return null;
        }
        rect.sizeDelta = new Vector2(_width*sizeMultiplier,_height*sizeMultiplier);
    }

}
