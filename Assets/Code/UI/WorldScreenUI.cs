using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldScreenUI : MonoBehaviour
{
    public static WorldScreenUI instance;
    Camera cam;
    public GameObject dialogueIcon;
    public GameObject bookIcon;
    public GameObject pushIcon;
    private void Awake() {
        if(instance==null)
        {
            instance = this;
        }
        else Destroy(this);
        cam = Camera.main;
    }
    public void SetIcon(IconType iconType,Vector3 _pos)
    {
        GameObject targetIcon;
        switch (iconType)
        {
            case IconType.Dialogue:
            targetIcon = dialogueIcon;
            break;

            case IconType.Book:
            targetIcon = bookIcon;
            break;

            default:
            targetIcon = pushIcon;
            break;
        }

        targetIcon.SetActive(true);
        Vector3 newPos = cam.WorldToScreenPoint(_pos);
        if(targetIcon.transform.position!= newPos) targetIcon.transform.position = newPos;
    }
    public void HideIcon(IconType iconType)
    {
        switch (iconType)
        {
            case IconType.Dialogue:
            dialogueIcon.SetActive(false);
            break;

            case IconType.Book:
            bookIcon.SetActive(false);
            break;

            default:
            pushIcon.SetActive(false);
            break;
        }
    }
}
public enum IconType
{
    Dialogue,
    Book,
    Push
}
