using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EventMonsterMirror : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Transform eyeTransform;
    public float delayToKill;

    public UnityEvent eventToDo;

    private void Start()
    {
        lineRenderer.gameObject.SetActive(false);
    }
    public void ThrowToMonster()
    {
        lineRenderer.gameObject.SetActive(true);
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, eyeTransform.position);
        StartCoroutine(EventMonster());
    }

    IEnumerator EventMonster()
    {
        yield return new WaitForSeconds(delayToKill);
        eyeTransform.gameObject.SetActive(false);
        eventToDo?.Invoke();
    }

}
