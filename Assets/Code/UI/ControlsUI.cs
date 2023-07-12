using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControlsUI : MonoBehaviour
{
    public static ControlsUI instance;
    public List<Image> bookImages;
    public float timeToDisplay;
    public float speed;
    bool onScreen;
    private void Awake() {
        if(instance==null) instance = this;
        else Destroy(this);
    }
    void Start()
    {
        SetImagesAlpha(bookImages,0);
    }
    public void ShowBookControls()
    {
        if(onScreen) return;
        StopAllCoroutines();
        onScreen = true;
        StartCoroutine(ShowBookControlsCoroutine());
    }
    public void HideBookControls()
    {
        if(!onScreen) return;
        StopAllCoroutines();
        onScreen = false;
        StartCoroutine(HideBookControlsCoroutine());
    }
    void SetAlpha(Image image, float alpha)
    {
        image.color = new Color(1,1,1,alpha);
    }
    IEnumerator ShowBookControlsCoroutine()
    {
        while(bookImages[0].color.a < 1)
        {
            SetImagesAlpha(bookImages,bookImages[0].color.a + speed*Time.deltaTime);
            yield return null;
        }
        SetImagesAlpha(bookImages,1);
    }
    IEnumerator HideBookControlsCoroutine()
    {
        while(bookImages[0].color.a > 0)
        {
            SetImagesAlpha(bookImages,bookImages[0].color.a - speed*Time.deltaTime);
            yield return null;
        }
        SetImagesAlpha(bookImages,0);
    }
    
    void SetImagesAlpha(List<Image> images, float alpha)
    {
        foreach (Image item in images)
        {
            SetAlpha(item,alpha);
        }
    }
}
