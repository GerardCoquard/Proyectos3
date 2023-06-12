using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIRoomNames : MonoBehaviour
{
    public static UIRoomNames instance;
    public float timeOnScreen;
    public float fadeTime;
    public List<TextMeshProUGUI> names;
    private void Awake() {
        if(instance==null)
        {
            instance = this;
        }
        else Destroy(this);
    }
    private void Start() {
        foreach (TextMeshProUGUI item in names)
        {
            item.alpha = 0;
        }
    }
    public void ShowText(int roomNumber)
    {
        StartCoroutine(Show(names[roomNumber]));
    }
    IEnumerator Show(TextMeshProUGUI text)
    {
        float time = 0;
        while (time<fadeTime)
        {
            text.alpha = Mathf.Lerp(0,1,time/fadeTime);
            yield return null;
        }
        text.alpha=1;
        time = 0;

        yield return new WaitForSeconds(timeOnScreen);

        while (time<fadeTime)
        {
            text.alpha = Mathf.Lerp(1,0,time/fadeTime);
            yield return null;
        }
        text.alpha=0;
    }
}
