using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EventMonsterMirror : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Transform eyeTransform;
    public float delayToKill;
    public float delayToDisapear;
    private bool isFinish;
    public UnityEvent eventToDo;

    private void Start()
    {
        isFinish = false;
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
        eventToDo?.Invoke();
        yield return new WaitForSeconds(delayToDisapear);
        lineRenderer.gameObject.SetActive(false);
        eyeTransform.gameObject.SetActive(false);
    }

}
