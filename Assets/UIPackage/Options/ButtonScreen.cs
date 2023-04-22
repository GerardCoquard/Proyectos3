using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScreen : MonoBehaviour
{
    public GameObject screen;
    RectTransform rectTransform;
    float _width;
    float _height;
    public float GetWidth()
    {
        if(_width==0) _width = GetRect().rect.width;
        return _width;
    }
    public float GetHeight()
    {
        if(_height==0) _height = GetRect().rect.height;
        return _height;
    }
    public RectTransform GetRect()
    {
        if(rectTransform==null) rectTransform = GetComponent<RectTransform>();
        return rectTransform;
    }
}
