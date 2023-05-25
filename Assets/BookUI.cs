using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BookUI : MonoBehaviour
{
    public static BookUI instance;
    public TextMeshProUGUI text;
    public Transform startPos;
    public float timeOnScreen;
    public float timeToFadeOut;
    public float distance;
    private void Awake() {
        if(instance==null) instance = this;
        else Destroy(this);
    }
    private void Start() {
        text.gameObject.SetActive(false);
    }
    public void ShowWarning()
    {
        StartCoroutine(ShowText());
    }
    IEnumerator ShowText()
    {
        Vector3 finalPos = startPos.position + new Vector3(0,startPos.position.y*(1+distance),0);
        float timer = 0;
        text.gameObject.SetActive(true);
        text.transform.position = startPos.position;
        text.alpha = 1f;
        while (timer<timeOnScreen)
        {
            text.transform.position = Vector3.Lerp(startPos.position,finalPos,timer/timeOnScreen);
            if(timer > timeToFadeOut)
            {
                text.alpha = Mathf.Lerp(1,0,(timer-timeToFadeOut)/(timeOnScreen-timeToFadeOut));
            }
            timer+= Time.deltaTime;
            yield return null;
        }
        text.gameObject.SetActive(false);
    }
}
