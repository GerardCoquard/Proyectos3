using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntermittentEffect : MonoBehaviour
{
    private Image image;
    public float time;

    private void OnEnable()
    {
        image = GetComponent<Image>();
        StartCoroutine(Intermittent());
    }

    IEnumerator Intermittent()
    {

        while(gameObject.activeSelf == true)
        {
            yield return new WaitForSeconds(time);
            Effect(true);
        }
    }

    private void Effect(bool state)
    {
        state = state == false ? true : false;
        image.enabled = state;
        Effect(state);

    }
}
