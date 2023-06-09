using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TickEffect : MonoBehaviour
{
    private Image myImage;
    public float speed = 2f;

    private void Start()
    {
        myImage = GetComponent<Image>();
        StartCoroutine(AlphaDown());
    }
    IEnumerator AlphaUp()
    {
        float delta = 0f;
        while(myImage.color.a <= 1f)
        {
            delta += Time.deltaTime * speed;
            myImage.color = new Color(myImage.color.r, myImage.color.g, myImage.color.b, delta);
            yield return null;
        }
        StartCoroutine(AlphaDown());
    }

    IEnumerator AlphaDown()
    {
        float delta = 1f;
        while (myImage.color.a >= 0f)
        {
            delta -= Time.deltaTime * speed;
            myImage.color = new Color(myImage.color.r, myImage.color.g, myImage.color.b, delta);
            yield return null;
        }
        StartCoroutine(AlphaUp());
    }
}
